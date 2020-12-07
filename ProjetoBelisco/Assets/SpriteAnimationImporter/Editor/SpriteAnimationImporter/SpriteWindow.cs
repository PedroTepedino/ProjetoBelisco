using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace SpriteAnimationImporter
{
    [ExecuteInEditMode]
    public class SpriteWindow : EditorWindow
    {
        private GUIStyle m_pivotStyle;
        private ImporterSettings0.SpriteSheetSettings m_settings;

        private string m_atlasPath;
        private string m_atlasName;
        private TextureImporter m_importer;
        private Texture2D m_atlas;
        private AnimationImporter m_animImporter;

        // user accessible settings
        private TextAsset m_animationInfo;
        private AnimationImporter.ImporterType m_importerType;
        private bool m_trimWidth, m_trimHeight;
        private bool m_adjustXPivot, m_adjustYPivot;
        private Vector2 m_defaultPivot;
        private bool m_useTagNames;
        private bool m_prefixClipsWithAtlasName;
        private int m_defaultSampleRate;
        private bool m_saveClipsToCustomDirectory;
        private DefaultAsset m_clipSaveDirectory;

        private bool m_isPlayingAnim;
        private Vector2 m_spriteListScrollbar, m_animListScrollbar;
        private int m_spriteListSelectedIdx, m_animListSelectedIdx;
        private float m_animPreviewTimer = 0.0f;
        private double m_lastUpdateTime = 0.0;

        [MenuItem("Assets/Animation Importer", validate = true)]
        public static bool ImportAnimationCheck()
        {
            if (Selection.activeObject == null || !(Selection.activeObject is Texture2D))
                return false;
            else
            {
                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
                TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
                return importer.textureType == TextureImporterType.Sprite
                    && importer.spriteImportMode == SpriteImportMode.Multiple;
            }
        }

        [MenuItem("Assets/Animation Importer", priority = 1001)]
        public static void ImportAnimation()
        {
            var window = GetWindow(typeof(SpriteWindow)) as SpriteWindow;
            window.Open(AssetDatabase.GetAssetPath(Selection.activeObject));
        }

        /// <summary>
        /// Sets all fields to default values
        /// </summary>
        private void ResetFields()
        {
            if(m_pivotStyle == null)
            {
                var tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, new Color(1.0f, 0.0f, 1.0f, 1.0f));
                tex.Apply();

                m_pivotStyle = new GUIStyle();
                m_pivotStyle.normal.background = tex;
            }
            m_atlasPath = null;
            m_atlasName = null;
            m_importer = null;
            m_atlas = null;
            m_spriteListScrollbar = new Vector2();
            m_animListScrollbar = new Vector2();
            m_spriteListSelectedIdx = 0;
            m_animListSelectedIdx = 0;
            m_animPreviewTimer = 0.0f;
            m_lastUpdateTime = 0.0f;
            m_isPlayingAnim = false;

            // user settings
            m_animationInfo = null;
            SetAnimationImporter(AnimationImporter.ImporterType.Aseprite);
            m_trimWidth = false;
            m_trimHeight = false;
            m_adjustXPivot = true;
            m_adjustYPivot = true;
            m_defaultPivot = new Vector2(0.5f, 0.5f);
            m_useTagNames = true;
            m_prefixClipsWithAtlasName = true;
            m_defaultSampleRate = 60;
            m_saveClipsToCustomDirectory = false;
            m_clipSaveDirectory = null;
        }

        private void SetAnimationImporter(AnimationImporter.ImporterType importerType)
        {
            m_importerType = importerType;
            m_animImporter = AnimationImporter.ImporterFactory(m_importerType);
        }

        /// <summary>
        /// Opens a sprite sheet in this window
        /// </summary>
        private void Open(string atlasPath)
        {
            // if we're switching sprite sheets...
            if (m_atlasPath != null)
            {
                // make sure to save previous sprite sheet's data
                WriteSettings();
                ImporterSettingsManager.Instance.SaveSettings();
            }

            ResetFields();
            m_atlasPath = atlasPath;

            // find spritesheet's importer
            m_importer = AssetImporter.GetAtPath(m_atlasPath) as TextureImporter;
            // find spritesheet's texture
            m_atlas = AssetDatabase.LoadAssetAtPath<Texture2D>(m_atlasPath);
            m_atlasName = Path.GetFileNameWithoutExtension(m_atlasPath);
            // try to find animation info as a .json file next to atlas.
            string metaDataPath = Path.GetDirectoryName(m_atlasPath) + "/" + m_atlasName + ".json";
            m_animationInfo = AssetDatabase.LoadAssetAtPath<TextAsset>(metaDataPath);
            // load settings stored for this spritesheet
            LoadSettings();
        }

        /// <summary>
        /// Loads settings for current sprite sheet from settings file
        /// </summary>
        private void LoadSettings()
        {
            var importerSettings = ImporterSettingsManager.Instance.Settings;

            // if we already have some settings saved for this sprite sheet...
            if (importerSettings.SpritesheetPaths.Contains(m_atlasPath))
            {
                var spritesheetSettingsIdx = importerSettings.FindSpritesheetSettings(m_atlasPath);
                m_settings = importerSettings.spritesheets[spritesheetSettingsIdx];
                // load settings for this sprite sheet
                if (m_settings.animationInfo != null)
                {
                    var newAnimInfo = AssetDatabase.LoadAssetAtPath<TextAsset>(m_settings.animationInfo);
                    if (newAnimInfo != null)
                        m_animationInfo = newAnimInfo;
                }
                SetAnimationImporter((AnimationImporter.ImporterType)System.Enum.Parse(
                    typeof(AnimationImporter.ImporterType),
                    m_settings.importerType));
                // load animation information with fresh animation info
                RefreshMetaData();
                m_trimWidth = m_settings.trimWidth;
                m_trimHeight = m_settings.trimHeight;
                m_adjustXPivot = m_settings.adjustXPivot;
                m_adjustYPivot = m_settings.adjustYPivot;
                m_defaultPivot = m_settings.defaultPivot;
                m_useTagNames = m_settings.useTagNames;
                m_prefixClipsWithAtlasName = m_settings.prefixClipsWithAtlasName;
                m_defaultSampleRate = m_settings.defaultSampleRate;
                m_saveClipsToCustomDirectory = m_settings.saveClipsToCustomDirectory;
                if (m_settings.clipSaveDirectory != null)
                    m_clipSaveDirectory = AssetDatabase.LoadAssetAtPath<DefaultAsset>(m_settings.clipSaveDirectory);
                else
                    m_clipSaveDirectory = null;

                // load settings for animation clips
                for (int i = 0; i < m_settings.animSettings.Count; i++)
                {
                    var animTag = m_settings.animSettings[i].tagName;

                    // find referenced animation by tag
                    SpriteAnimation anim = null;
                    for (int j = 0; j < m_animImporter.Animations.Length; j++)
                    {
                        if (m_animImporter.Animations[j].name == animTag)
                        {
                            anim = m_animImporter.Animations[j];
                            break;
                        }
                    }

                    // if we found referenced animation, load settings
                    if (anim != null)
                    {
                        var animSetting = m_settings.animSettings[i];
                        anim.name = animTag;
                        anim.loop = animSetting.loop;
                        anim.sampleRate = animSetting.sampleRate;
                        anim.useDefaultSampleRate = animSetting.useDefaultSampleRate;
                        anim.generateClip = animSetting.generateClip;
                    }
                    // since we haven't found referenced animation clip, we might as well drop it
                    else
                    {
                        m_settings.animSettings.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                // No settings for given spritesheet were found. It's possible it was moved.
                // Iterate over saved spritesheet settings and find any matching file names
                var matchingSaved = new List<string>();
                foreach (var sheet in importerSettings.SpritesheetPaths)
                    if (Path.GetFileNameWithoutExtension(sheet) == m_atlasName)
                        matchingSaved.Add(sheet);
                if(matchingSaved.Count > 0)
                {
                    // give user option to import settings from found matching spritesheet names
                    foreach (var saved in matchingSaved)
                    {
                        if(EditorUtility.DisplayDialog("Import settings",
                            "Settings were found for this spritesheet name, at " + saved + ". Import them?", "yes", "no"))
                        {
                            int idx = importerSettings.FindSpritesheetSettings(saved);
                            var settings = importerSettings.spritesheets[idx];
                            settings.spritesheetPath = m_atlasPath;
                            importerSettings.spritesheets[idx] = settings;
                            LoadSettings();
                            return;
                        }
                    }
                }

                // otherwise create new settings
                m_settings = new ImporterSettings0.SpriteSheetSettings();
                RefreshMetaData();
            }
        }

        /// <summary>
        /// Preserves settings for current spritesheet.
        /// </summary>
        private void WriteSettings()
        {
            m_settings.spritesheetPath = m_atlasPath;
            if (m_animationInfo)
                m_settings.animationInfo = AssetDatabase.GetAssetPath(m_animationInfo);
            m_settings.importerType = m_importerType.ToString();
            m_settings.trimWidth = m_trimWidth;
            m_settings.trimHeight = m_trimHeight;
            m_settings.adjustXPivot = m_adjustXPivot;
            m_settings.adjustYPivot = m_adjustYPivot;
            m_settings.defaultPivot = m_defaultPivot;
            m_settings.useTagNames = m_useTagNames;
            m_settings.prefixClipsWithAtlasName = m_prefixClipsWithAtlasName;
            m_settings.defaultSampleRate = m_defaultSampleRate;
            m_settings.saveClipsToCustomDirectory = m_saveClipsToCustomDirectory;
            if (m_clipSaveDirectory != null)
                m_settings.clipSaveDirectory = AssetDatabase.GetAssetPath(m_clipSaveDirectory);
            else
                m_settings.clipSaveDirectory = null;
            m_settings.animSettings = new List<ImporterSettings0.SpriteAnimSettings>();

            // save animation settings
            for(int i = 0; i < m_animImporter.Animations.Length; i++)
            {
                var animSettings = new ImporterSettings0.SpriteAnimSettings();
                animSettings.tagName = m_animImporter.Animations[i].name;
                animSettings.loop = m_animImporter.Animations[i].loop;
                animSettings.sampleRate = m_animImporter.Animations[i].sampleRate;
                animSettings.useDefaultSampleRate = m_animImporter.Animations[i].useDefaultSampleRate;
                animSettings.generateClip = m_animImporter.Animations[i].generateClip;
                int animIndex = m_settings.FindAnimSettings(m_animImporter.Animations[i].name);
                if (animIndex != -1)
                    m_settings.animSettings[animIndex] = animSettings;
                else
                    m_settings.animSettings.Add(animSettings);
            }

            var spritesheetSettingsIdx = ImporterSettingsManager.Instance.Settings.FindSpritesheetSettings(m_atlasPath);
            if (spritesheetSettingsIdx != -1)
                ImporterSettingsManager.Instance.Settings.spritesheets[spritesheetSettingsIdx] = m_settings;
            else
                ImporterSettingsManager.Instance.Settings.spritesheets.Add(m_settings);
        }

        /// <summary>
        /// Loads spritesheet animation / frames meta data and sets frame names.
        /// </summary>
        private void RefreshMetaData()
        {
            m_animImporter.Load(m_animationInfo, m_atlas);
            m_animImporter.SetFrameNames(m_atlas.name, m_useTagNames);
        }

        private void DrawTrimSetting()
        {
            GUILayout.Label(new GUIContent("Trim sprites", "If enabled, will trim sprite frames to visible content"), GUILayout.Width(145.0f));
            EditorGUILayout.BeginHorizontal();
            m_trimWidth = GUILayout.Toggle(m_trimWidth, "width", GUILayout.ExpandWidth(false));
            GUI.enabled = m_trimWidth;
            m_adjustXPivot = GUILayout.Toggle(m_adjustXPivot, new GUIContent("adjust pivot", "If enabled, will offset pivot of a trimmed sprite to make sure it stays in the same place"), GUILayout.ExpandWidth(false));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            m_trimHeight = GUILayout.Toggle(m_trimHeight, "height", GUILayout.ExpandWidth(false));
            GUI.enabled = m_trimHeight;
            m_adjustYPivot = GUILayout.Toggle(m_adjustYPivot, new GUIContent("adjust pivot", "If enabled, will offset pivot of a trimmed sprite to make sure it stays in the same place"), GUILayout.ExpandWidth(false));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws sprite preview box.
        /// </summary>
        private void DrawPreviewSetting()
        {
            // draw the space for previewing the sprite
            const float previewWidth = 100.0f, previewHeight = 100.0f;
            const float pivotWidth = 10.0f, pivotHeight = 10.0f;

            GUILayout.Box("", new GUILayoutOption[] { GUILayout.Width(previewWidth), GUILayout.Height(previewHeight) });

            if (m_animImporter.Frames.Length > 0 && m_spriteListSelectedIdx >= 0 && m_spriteListSelectedIdx < m_animImporter.Frames.Length
                && m_animImporter.Animations.Length > 0 && m_animListSelectedIdx >= 0 && m_animListSelectedIdx < m_animImporter.Animations.Length)
            {
                var currentFrame = m_animImporter.Animations[m_animListSelectedIdx].frames[m_spriteListSelectedIdx];
                var previewRect = GUILayoutUtility.GetLastRect();
                var previewSpriteRect = previewRect;
                // preserve aspect ratio
                if (currentFrame.rect.width > currentFrame.rect.height)
                {
                    previewSpriteRect.height *= currentFrame.rect.height / currentFrame.rect.width;
                    previewSpriteRect.y += (previewHeight - previewSpriteRect.height) / 2;
                }
                else if (currentFrame.rect.height > currentFrame.rect.width)
                {
                    previewSpriteRect.width *= currentFrame.rect.width / currentFrame.rect.height;
                    previewSpriteRect.x += (previewWidth - previewSpriteRect.width) / 2;
                }
                Rect texcoords = new Rect(
                    currentFrame.rect.x / m_atlas.width,
                    (currentFrame.rect.y - currentFrame.rect.height + 1) / m_atlas.height,
                    currentFrame.rect.width / m_atlas.width,
                    currentFrame.rect.height / m_atlas.height
                );
                GUI.DrawTextureWithTexCoords(previewSpriteRect, m_atlas, texcoords);

                // draw pivot
                float yOffset = previewRect.height * m_defaultPivot.y;
                float xOffset = previewRect.width * m_defaultPivot.x;
                // horizontal stripe
                GUI.Box(new Rect(previewRect.x + xOffset - pivotWidth, previewRect.y + previewRect.height - yOffset, pivotWidth * 2, 1.0f),
                    GUIContent.none, m_pivotStyle);
                // vertical stripe
                GUI.Box(new Rect(previewRect.x + xOffset, previewRect.y + previewRect.height - yOffset - pivotHeight, 1.0f, pivotHeight * 2),
                    GUIContent.none, m_pivotStyle);
            }

            if (GUILayout.Button(new GUIContent(m_isPlayingAnim ? "Stop" : "Preview", "Runs a preview of selected animation"),
                GUILayout.Width(100.0f)))
            {
                m_isPlayingAnim = !m_isPlayingAnim;
            }
        }

        private void OnGUI()
        {
            EditorGUIUtility.wideMode = true;

            ///////////////////////////////////////////////////////
            // SETTINGS
            ///////////////////////////////////////////////////////
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            var newAnimInfo = EditorGUILayout.ObjectField(new GUIContent("Meta data", "Animation meta data text file"), m_animationInfo, typeof(TextAsset), false) as TextAsset;
            if (newAnimInfo != m_animationInfo)
            {
                m_animationInfo = newAnimInfo;
                RefreshMetaData();
            }

            var importerType = (AnimationImporter.ImporterType)EditorGUILayout.EnumPopup("Meta data format", m_importerType);
            if(importerType != m_importerType)
            {
                WriteSettings();
                SetAnimationImporter(importerType);
                LoadSettings();
            }

            m_saveClipsToCustomDirectory = !EditorGUILayout.Toggle(
                new GUIContent("Save clips next to sprite", "If enabled, generated animation clips will be placed next to spritesheet"),
                !m_saveClipsToCustomDirectory,
                GUILayout.ExpandWidth(false));
            GUI.enabled = m_saveClipsToCustomDirectory;
            GUI.SetNextControlName("m_clipSaveDirectory");
            m_clipSaveDirectory = (DefaultAsset)EditorGUILayout.ObjectField(
                new GUIContent("Save clips to", "Determines the directory animation clips will be saved to"),
                m_clipSaveDirectory,
                typeof(DefaultAsset),
                false
            );
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            m_defaultPivot = EditorGUILayout.Vector2Field(new GUIContent("Pivot", "Pivot to apply for every sprite in atlas"),
                m_defaultPivot, GUILayout.ExpandWidth(true));
            if(GUILayout.Button(new GUIContent("calculate center", "Finds center in currently selected trimmed sprite and sets it as pivot"),
                GUILayout.ExpandWidth(false)))
            {
                if(m_animImporter.Frames.Length > 0 && m_spriteListSelectedIdx >= 0 && m_spriteListSelectedIdx < m_animImporter.Frames.Length
                    && m_animImporter.Animations.Length > 0 && m_animListSelectedIdx >= 0 && m_animListSelectedIdx < m_animImporter.Animations.Length)
                {
                    // calculate center for selected, trimmed sprite
                    var currentFrame = m_animImporter.Animations[m_animListSelectedIdx].frames[m_spriteListSelectedIdx];
                    Rect trimmed = currentFrame.GetTrimmed(GetReadableAtlas(), true, true);
                    float trimmedX = trimmed.x + trimmed.width * 0.5f;
                    float trimmedY = trimmed.y - trimmed.height * 0.5f;
                    m_defaultPivot = new Vector2(
                        (trimmedX - currentFrame.rect.x) / currentFrame.rect.width,
                        (trimmedY - currentFrame.rect.y + currentFrame.rect.height) / currentFrame.rect.height
                    );
                }
            }
            EditorGUILayout.EndHorizontal();
            DrawTrimSetting();
            m_prefixClipsWithAtlasName = EditorGUILayout.Toggle(new GUIContent("Prefix clip names", "If enabled, exported clip names will begin with atlas name"),
                m_prefixClipsWithAtlasName);
            m_defaultSampleRate = EditorGUILayout.IntField(new GUIContent("Default sample rate", "Sample rate to be applied to all animation clips"),
                m_defaultSampleRate, GUILayout.ExpandWidth(false));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            ///////////////////////////////////////////////////////
            // PREVIEW
            ///////////////////////////////////////////////////////
            EditorGUILayout.BeginVertical();
            DrawPreviewSetting();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            ///////////////////////////////////////////////////////
            // CLIPS
            ///////////////////////////////////////////////////////
            GUILayout.Label("Clips", EditorStyles.boldLabel);
            // draw sprite and animation list boxes
            EditorGUILayout.BeginHorizontal();
            // draw animation list
            m_animListScrollbar = EditorGUILayout.BeginScrollView(m_animListScrollbar, EditorStyles.helpBox, GUILayout.Width(position.width * 0.4f));
            GUIContent[] animContents = new GUIContent[m_animImporter.Animations.Length];
            for (int i = 0; i < m_animImporter.Animations.Length; i++)
            {
                animContents[i] = new GUIContent(m_animImporter.Animations[i].name + " ("
                    + m_animImporter.Animations[i].frames.Count + " frames)");
            }
            GUI.SetNextControlName("___focusFix");
            int newAnimListIdx = GUILayout.SelectionGrid(m_animListSelectedIdx, animContents, 1);
            if(newAnimListIdx != m_animListSelectedIdx)
            {
                m_spriteListSelectedIdx = 0;
                m_animListSelectedIdx = newAnimListIdx;
                m_isPlayingAnim = false;
                // we need to focus something when we change animation clips
                // otherwise focused values won't update with new animation's values (eg. sample rate input)
                GUI.FocusControl("___focusFix");
            }
            EditorGUILayout.EndScrollView();
            // sprite list & clip options
            if (m_animImporter.Frames.Length > 0 && m_spriteListSelectedIdx >= 0 && m_spriteListSelectedIdx < m_animImporter.Frames.Length)
            {
                var anim = m_animImporter.Animations[m_animListSelectedIdx];
                EditorGUILayout.BeginVertical();
                ///////////////////////////////////////////////////////
                // CLIP OPTIONS
                ///////////////////////////////////////////////////////
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(false));
                EditorGUILayout.BeginHorizontal();
                anim.generateClip = EditorGUILayout.Toggle(new GUIContent("Generate clip", "If disabled, no animation clip will be generated for this animation"),
                    anim.generateClip, GUILayout.ExpandWidth(false));
                anim.loop = EditorGUILayout.Toggle(new GUIContent("Loop", "If enabled, clip will loop"), anim.loop, GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                anim.useDefaultSampleRate = EditorGUILayout.Toggle("Use default sample rate", anim.useDefaultSampleRate, GUILayout.ExpandWidth(false));
                GUI.enabled = !anim.useDefaultSampleRate;
                anim.sampleRate = EditorGUILayout.IntField(new GUIContent("Samples", "The sample rate of this clip"),
                    anim.useDefaultSampleRate ? m_defaultSampleRate : anim.sampleRate, GUILayout.ExpandWidth(false));
                GUI.enabled = true;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                ///////////////////////////////////////////////////////
                // FRAMES LIST
                ///////////////////////////////////////////////////////
                m_spriteListScrollbar = EditorGUILayout.BeginScrollView(m_spriteListScrollbar, EditorStyles.helpBox, GUILayout.ExpandWidth(true));
                GUIContent[] frameContents = new GUIContent[anim.frames.Count];
                for (int i = 0; i < anim.frames.Count; i++)
                {
                    frameContents[i] = new GUIContent("frame (" + anim.frames[i].id + "): " + anim.frames[i].name
                        + " (duration: " + anim.frames[i].duration + "ms)");
                }
                m_spriteListSelectedIdx = GUILayout.SelectionGrid(m_spriteListSelectedIdx, frameContents, 1);
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();

            ///////////////////////////////////////////////////////
            // CREATING SPRITES & CLIPS
            ///////////////////////////////////////////////////////
            if (GUILayout.Button("Create sprites & clips", GUILayout.Height(36.0f)))
            {
                if (!m_animationInfo)
                {
                    EditorUtility.DisplayDialog("Animation meta data missing",
                        "Please select an animation meta data file before creating sprites", "ok");
                }
                else if (m_saveClipsToCustomDirectory && m_clipSaveDirectory == null)
                {
                    // highlight misconfigured dialog
                    GUI.FocusControl("m_clipSaveDirectory");
                    EditorUtility.DisplayDialog("No clip save directory chosen",
                        "Custom directory output was enabled, but no directory was chosen", "ok");
                }
                else
                {
                    CreateSprites();
                    CreateAnimationClips();
                }
            }
        }

        private void Update()
        {
            if(m_isPlayingAnim && m_animImporter.Frames.Length > 0 && m_animImporter.Animations.Length > 0)
            {
                m_animPreviewTimer += (float)(EditorApplication.timeSinceStartup - m_lastUpdateTime);
                var currentAnim = m_animImporter.Animations[m_animListSelectedIdx];
                var currentFrame = currentAnim.frames[m_spriteListSelectedIdx];
                bool shouldRepaint = false;
                while (m_animPreviewTimer > currentFrame.duration / 1000.0f)
                {
                    m_spriteListSelectedIdx += 1;
                    m_spriteListSelectedIdx %= currentAnim.frames.Count;
                    m_animPreviewTimer -= currentFrame.duration / 1000.0f;
                    currentFrame = currentAnim.frames[m_spriteListSelectedIdx];
                    shouldRepaint = true;
                }
                if (shouldRepaint)
                    Repaint();
            }
            m_lastUpdateTime = EditorApplication.timeSinceStartup;
        }

        private Texture2D GetReadableAtlas()
        {
            Texture2D tmpAtlas = m_atlas;
            if (!m_importer.isReadable)
            {
                // create a temporary copy of atlas Texture2D so we can read pixels from it
                // when trimming without having to set the texture as readable
                RenderTexture tmpRt = RenderTexture.GetTemporary(
                        m_atlas.width,
                        m_atlas.height,
                        0,
                        RenderTextureFormat.ARGB32,
                        RenderTextureReadWrite.Linear);
                Graphics.Blit(m_atlas, tmpRt);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = tmpRt;
                tmpAtlas = new Texture2D(m_atlas.width, m_atlas.height);
                tmpAtlas.filterMode = m_atlas.filterMode;
                tmpAtlas.wrapMode = TextureWrapMode.Clamp;
                tmpAtlas.ReadPixels(new Rect(0, 0, tmpAtlas.width, tmpAtlas.height), 0, 0);
                tmpAtlas.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(tmpRt);
            }
            return tmpAtlas;
        }

        private void CreateSprites()
        {
            Texture2D tmpAtlas = GetReadableAtlas();
            var sprites = new SpriteMetaData[m_animImporter.Frames.Length];
            // create sprites
            for(int i = 0; i < sprites.Length; i++)
            {
                var sprite = new SpriteMetaData();
                sprite.name = m_animImporter.Frames[i].name;
                sprite.rect = m_animImporter.Frames[i].GetTrimmed(tmpAtlas, m_trimWidth, m_trimHeight);
                sprite.alignment = (int)SpriteAlignment.Custom;
                sprite.pivot = m_animImporter.Frames[i].GetTrimmedPivot(sprite.rect, m_defaultPivot, m_adjustXPivot, m_adjustYPivot);
                sprite.rect = new Rect(sprite.rect.x, sprite.rect.y - sprite.rect.height + 1, sprite.rect.width, sprite.rect.height);
                sprites[i] = sprite;
            }

            m_importer.spritesheet = sprites;
            EditorUtility.SetDirty(m_importer);
            m_importer.SaveAndReimport();
        }

        private void CreateAnimationClips()
        {
            // find sprites in current spritesheet
            List<Sprite> spriteFrames = new List<Sprite>();
            foreach (var s in AssetDatabase.LoadAllAssetsAtPath(m_atlasPath))
                if (s is Sprite)
                    spriteFrames.Add(s as Sprite);

            string clipSavePath;
            if (m_saveClipsToCustomDirectory)
                clipSavePath = AssetDatabase.GetAssetPath(m_clipSaveDirectory) + "/";
            else
                clipSavePath = Path.GetDirectoryName(m_atlasPath) + "/";

            foreach(var anim in m_animImporter.Animations)
            {
                if (!anim.generateClip)
                    continue;

                string animName = anim.name;
                if (m_prefixClipsWithAtlasName)
                    animName = m_atlas.name + char.ToUpper(animName[0]) + animName.Substring(1);

                bool wasCreated = false;
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipSavePath + animName + ".anim");
                if (clip == null)
                {
                    wasCreated = true;
                    clip = new AnimationClip();
                }

                AnimationClipSettings clipSettings = new AnimationClipSettings();
                clipSettings.loopTime = anim.loop;
                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
                clip.frameRate = anim.useDefaultSampleRate ? m_defaultSampleRate : anim.sampleRate;

                EditorCurveBinding spriteBinding = new EditorCurveBinding();
                spriteBinding.type = typeof(SpriteRenderer);
                spriteBinding.path = "";
                spriteBinding.propertyName = "m_Sprite";
                ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[anim.frames.Count];
                float clipTime = 0.0f;
                for (int j = 0; j < spriteKeyFrames.Length; j++)
                {
                    spriteKeyFrames[j] = new ObjectReferenceKeyframe();
                    spriteKeyFrames[j].time = clipTime;
                    clipTime += anim.frames[j].duration / 1000.0f;
                    // find sprite in generated atlas that represents current frame
                    Sprite sprite = null;
                    foreach (var s in spriteFrames)
                    {
                        if (s.name == anim.frames[j].name)
                        {
                            sprite = s;
                            break;
                        }
                    }
                    spriteKeyFrames[j].value = sprite;
                }
                AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);

                if (wasCreated)
                    AssetDatabase.CreateAsset(clip, clipSavePath + animName + ".anim");
                else
                    EditorUtility.SetDirty(clip);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void OnDestroy()
        {
            WriteSettings();
            ImporterSettingsManager.Instance.SaveSettings();
        }
    }
}