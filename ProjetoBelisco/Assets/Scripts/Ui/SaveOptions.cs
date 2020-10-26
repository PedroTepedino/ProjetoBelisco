using UnityEngine;

namespace Belisco
{
    public class SaveOptions : MonoBehaviour
    {
        private static SaveOptions _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        protected virtual void OnEnable()
        {
            _instance = this;
            LoadAllOptions();
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public static void LoadAllOptions()
        {
            if (_instance == null)
                return;

            var optionsParameters = _instance.GetComponentsInChildren<OptionsParameter>();
            foreach (OptionsParameter param in optionsParameters) param.Load();
        }

        public static void SaveAllOptions()
        {
            if (_instance == null)
                return;

            var optionsParameters = _instance.GetComponentsInChildren<OptionsParameter>();
            foreach (OptionsParameter param in optionsParameters) param.Save();
        }
    }
}