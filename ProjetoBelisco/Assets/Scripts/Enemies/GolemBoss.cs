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
            timer += Time.deltaTime;
            if (timer > attack.attackSpeed)
            {
                if (actualState != "alert" && actualState != "attack" && actualState != "chase")
                {
                    this.stateMachine.ChangeState(new AlertState(this.gameObject));
                }    
            }
            else
            {
                if(actualState != "move" && actualState != "attack" && actualState != "chase" && actualState != "alert")
                {
                    this.stateMachine.ChangeState(new MoveState(this.gameObject));
                }
            }

            this.stateMachine.RunStateUpdate();
        }
    }
}
