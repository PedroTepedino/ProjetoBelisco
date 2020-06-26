using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScripts.StateMachine.States;

namespace GameScripts.Enemies
{
    public class GolemBoss : Controller
    {
        private bool getAlerted = false;
        private float timer = 0f;

        public override void Update() {
            if (targeting.hasTarget)
            {
                if(!getAlerted)
                {
                    getAlerted = true;
                    if(actualState != "alert")
                    {
                        this.stateMachine.ChangeState(new AlertState(this.gameObject));
                    }
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > attack.attackSpeed)
                    {
                        if (Vector2.Distance(this.transform.position, targeting.target.position) > attack.attackRange)
                        {
                            if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                            {
                                this.stateMachine.ChangeState(new ChaseState(this.gameObject));
                            }
                        }
                        else
                        {
                            if (actualState != "attack")
                            {
                                this.stateMachine.ChangeState(new AttackState(this.gameObject));
                                timer = 0;
                            }
                        }
                    }
                    else
                    {
                        if (actualState != "move" && actualState != "attack" && actualState != "chase" && actualState != "alert")
                        {
                            this.stateMachine.ChangeState(new MoveState(this.gameObject));
                        }
                    }
                }
            }
            else
            {
                ResetBoss();
            }

            this.stateMachine.RunStateUpdate();
        }

        private void ResetBoss(){
            if (actualState != "iddle")
            {
                getAlerted = false;
                this.stateMachine.ChangeState(new IddleState(this.gameObject));
            }
        }
    }
}
