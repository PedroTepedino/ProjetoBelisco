using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    public class RestartButton : Button
    {
        private static RestartButton _instance;

        public static bool Pressed => _instance != null && _instance.IsPressed();

        protected override void OnEnable()
        {
            _instance = this;
            base.OnEnable();
        }
    }
}