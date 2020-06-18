using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    public class OptionsButton : AbstractButton<OptionsButton>
    {
        protected override void OnEnable()
        {
            _instance = this;
            base.OnEnable();
        }
    }
}