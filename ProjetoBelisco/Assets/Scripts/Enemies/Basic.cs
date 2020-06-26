using GameScripts.StateMachine.States;
using UnityEngine;

namespace GameScripts.Enemies
{
    public class Basic : Controller
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
                        Debug.Log(actualState + " >lookingRange");
                    }
                }              
                else if(Vector2.Distance(this.transform.position, targeting.target.position) > attack.attackRange)
                {
                    if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                    {
                        this.stateMachine.ChangeState(new AlertState(this.gameObject));
                        Debug.Log(actualState);
                    }
                }
                else
                {
                    if (actualState != "attack")
                    {
                        this.stateMachine.ChangeState(new AttackState(this.gameObject));
                        Debug.Log(actualState);
                    }
                }
            }
            else
            {
                if(actualState != "move"){
                    this.stateMachine.ChangeState(new MoveState(this.gameObject));
                    Debug.Log(actualState + " no target");
                }
            }

            this.stateMachine.RunStateUpdate();
        }
    }
}