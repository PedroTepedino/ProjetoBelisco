using GameScripts.StateMachine.States;

namespace GameScripts.Enemies
{
    public class Basic : Controller
    {    
        public override void Update()
        {
            if (targeting.hasTarget)
            {
                if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                {
                    this.stateMachine.ChangeState(new AlertState(this.gameObject));
                }
            }
            this.stateMachine.RunStateUpdate();
        }
    }
}
