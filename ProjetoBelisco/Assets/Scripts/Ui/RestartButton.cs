namespace Belisco
{
    public class RestartButton : AbstractButton<RestartButton>
    {
        protected override void OnEnable()
        {
            _instance = this;
            base.OnEnable();
        }
    }
}