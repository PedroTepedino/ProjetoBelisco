using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class AlertState : IState
    {

        private GameObject ownerGameObject;
        private Transform target;
        private IEnemyStateMachine ownerController;
        private float timer;
        private float alertAnimationTime;

        public AlertState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerController = owner.GetComponent<IEnemyStateMachine>();        
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
