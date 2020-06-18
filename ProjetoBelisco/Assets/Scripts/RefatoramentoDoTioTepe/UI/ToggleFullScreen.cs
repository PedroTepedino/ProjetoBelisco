using UnityEngine;
using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    public class ToggleFullScreen : Toggle, OptionsParameter
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            Screen.fullScreen = value;
        }

        public void Load()
        {
            Debug.LogError("Has Key " + PlayerPrefs.HasKey("FullScreen"));
            if (PlayerPrefs.HasKey("FullScreen"))
            {
                this.isOn = (PlayerPrefs.GetInt("FullScreen") == 1);
            }
        }

        public void Save()
        {
            PlayerPrefs.SetInt("FullScreen", this.isOn ? 1 : 0);
            Debug.LogError("Has Key " + PlayerPrefs.HasKey("FullScreen"));
        }
    }
}