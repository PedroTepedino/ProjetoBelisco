using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    public class OptionsButton : Button
    {
        private static OptionsButton _instance;

        public static bool Pressed => _instance != null && _instance.IsPressed();

        protected override void OnEnable()
        {
            _instance = this;
            base.OnEnable();
        }
    }
}