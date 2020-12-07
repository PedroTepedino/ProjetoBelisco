namespace Belisco
{
    public class OptionsPanel : AbstractPanel
    {
        protected override void HandleGameStateChanged(IState state)
        {
            _panel.SetActive(state is Options);
        }
    }
}