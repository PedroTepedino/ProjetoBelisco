using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class GamePlayOptions : SaveOptions
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}