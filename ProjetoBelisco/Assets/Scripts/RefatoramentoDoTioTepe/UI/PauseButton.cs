using Rewired;

namespace RefatoramentoDoTioTepe
{
    public class PauseButton : AbstractButton<PauseButton>
    {
        protected override void OnEnable()
        {
            _instance = this;
            base.OnEnable();
        }
    }
}