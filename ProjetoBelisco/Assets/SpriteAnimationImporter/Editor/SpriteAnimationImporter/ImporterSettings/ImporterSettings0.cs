using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteAnimationImporter
{
    public class ImporterSettings0 : ScriptableObject
    {
        [System.Serializable]
        public struct SpriteAnimSettings
        {
            public string tagName;

            public bool useDefaultSampleRate;
            public int sampleRate;
            public bool loop;
            public bool generateClip;
        }

        [System.Serializable]
        public struct SpriteSheetSettings
        {
            public string spritesheetPath;

            public string animationInfo;
            public string importerType;
            public bool trimWidth, trimHeight;
            public bool adjustXPivot, adjustYPivot;
            public Vector2 defaultPivot;
            public bool useTagNames;
            public bool prefixClipsWithAtlasName;
            public int defaultSampleRate;
            public bool saveClipsToCustomDirectory;
            public string clipSaveDirectory;

            public List<SpriteAnimSettings> animSettings;

            public int FindAnimSettings(string tagName)
            {
                for (int i = 0; i < animSettings.Count; i++)
                    if (animSettings[i].tagName == tagName)
                        return i;
                return -1;
            }
        }

        public List<SpriteSheetSettings> spritesheets = new List<SpriteSheetSettings>();

        public List<string> SpritesheetPaths
        {
            get
            {
                var paths = new List<string>();
                foreach (var spritesheet in spritesheets)
                    paths.Add(spritesheet.spritesheetPath);
                return paths;
            }
        }

        public int FindSpritesheetSettings(string spritesheetPath)
        {
            for (int i = 0; i < spritesheets.Count; i++)
                if (spritesheets[i].spritesheetPath == spritesheetPath)
                    return i;
            return -1;
        }
    }
}