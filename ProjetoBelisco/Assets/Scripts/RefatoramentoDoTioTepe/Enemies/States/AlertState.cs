using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AlertState : IState
    {

        private GameObject ownerGameObject;
        private Transform target;
        private EnemyStateMachine ownerController;
        private float timer;
        private float alertAnimationTime;

        public AlertState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerController = owner.GetComponent<EnemyStateMachine>();        
        }

        public void OnEnter()
        {
            alertAnimationTime = ownerController.EnemyParameters.AlertAnimationTime;
        }

        public void OnExit()
        {
       
        }

        public void Tick()
        {
            timer += Time.deltaTime;
        
        }

        public bool TimeEnded()
        {
            return (timer > alertAnimationTime);
        }
    }
}
