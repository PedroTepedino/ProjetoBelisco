using UnityEngine;
using UnityEngine.UI;

namespace Belisco
{
    public class ToggleFullScreen : Toggle, OptionsParameter
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            onValueChanged.RemoveListener(OnValueChanged);
        }

        public void Load()
        {
            Debug.LogError("Has Key " + PlayerPrefs.HasKey("FullScreen"));
            if (PlayerPrefs.HasKey("FullScreen")) isOn = PlayerPrefs.GetInt("FullScreen") == 1;
        }

        public void Save()
        {
            PlayerPrefs.SetInt("FullScreen", isOn ? 1 : 0);
            Debug.LogError("Has Key " + PlayerPrefs.HasKey("FullScreen"));
        }

        private void OnValueChanged(bool value)
        {
            Screen.fullScreen = value;
        }
    }
}