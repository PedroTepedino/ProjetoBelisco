namespace Belisco
{
    public class PausePanel : AbstractPanel
    {
        protected override void HandleGameStateChanged(IState state)
        {
            _panel.SetActive(state is Pause);
        }
    }
}