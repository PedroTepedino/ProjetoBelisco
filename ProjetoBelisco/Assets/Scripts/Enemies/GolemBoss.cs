using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScripts.StateMachine.States;

namespace GameScripts.Enemies
{
    public class GolemBoss : Controller
    {
        private float timer = 0f;

        public override void Update() {
            if (targeting.hasTarget)
            {
                timer += Time.deltaTime;
                if (timer > attack.attackSpeed)
                {
                    if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                    {
                        this.stateMachine.ChangeState(new ChaseState(this.gameObject));
                    }
                    timer = 0;
                }
                else
                {
                    if (actualState != "move" && actualState != "attack" && actualState != "chase" && actualState != "alert")
                    {
                        this.stateMachine.ChangeState(new MoveState(this.gameObject));
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
                this.stateMachine.ChangeState(new IddleState(this.gameObject));
            }
        }
    }
}
