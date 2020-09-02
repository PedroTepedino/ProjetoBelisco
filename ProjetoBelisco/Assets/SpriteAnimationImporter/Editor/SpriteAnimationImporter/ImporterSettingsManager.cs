using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SpriteAnimationImporter
{
    public class ImporterSettingsManager
    {
        private static ImporterSettingsManager m_instance = null;

        public static ImporterSettingsManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new ImporterSettingsManager();
                return m_instance;
            }
        }

        private const string k_settingsPath = "Assets/SpriteAnimationImporter/Editor/SpriteAnimationImporter/ImporterSettings.asset";
        private ImporterSettings0 m_settings = null;
        public ImporterSettings0 Settings
        {
            get
            {
                if (m_settings != null)
                    return m_settings;
                else
                {
                    LoadSettings();
                    return m_settings;
                }
            }
        }

        public ImporterSettingsManager()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            // since right now there's only one version of settings, we know we can try to load
            // only one type. Otherwise we'd need to try all the other types and do a migration.
            m_settings = AssetDatabase.LoadAssetAtPath<ImporterSettings0>(k_settingsPath);
            if (m_settings == null)
            {
                m_settings = ImporterSettings0.CreateInstance<ImporterSettings0>();
                AssetDatabase.CreateAsset(m_settings, k_settingsPath);
                AssetDatabase.SaveAssets();
            }
        }

        public void SaveSettings()
        {
            if (m_settings != null)
            {
                EditorUtility.SetDirty(m_settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}