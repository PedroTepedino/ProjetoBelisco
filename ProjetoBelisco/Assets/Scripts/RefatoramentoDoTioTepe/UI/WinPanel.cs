namespace RefatoramentoDoTioTepe
{
    public class WinPanel : AbstractPanel
    {

        protected override void HandleGameStateChanged(IState state)
        {
            _panel.SetActive(state is WinState);
        }
        
    }
}