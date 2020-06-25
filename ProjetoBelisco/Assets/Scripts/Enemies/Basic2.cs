using GameScripts.StateMachine.States;

namespace GameScripts.Enemies
{
    public class Basic2 : Controller
    {    
        public override void Update()
        {
            if (targeting.hasTarget)
            {
                if(Vector2.Distance(this.transform.position, targeting.target.position) > lookingRange)
                {
                    if(actualState != "move")
                    {
                        this.stateMachine.ChangeState(new MoveState(this.gameObject));
                    }
                }              
                else if(Vector2.Distance(this.transform.position, targeting.target.position) > attack.attackRange)
                {
                    if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                    {
                        this.stateMachine.ChangeState(new AlertState(this.gameObject));
                    }
                }
                else
                {
                    if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                    {
                        this.stateMachine.ChangeState(new AttackState(this.gameObject));
                    }
                }
            }
            else
            {
                if(actualState != "move"){
                    this.stateMachine.ChangeState(new MoveState(this.gameObject));
                }
            }

            this.stateMachine.RunStateUpdate();
        }
    }
}