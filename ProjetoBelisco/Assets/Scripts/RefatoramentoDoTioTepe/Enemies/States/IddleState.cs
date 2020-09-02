using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class IddleState : IState
    {
        private GameObject ownerGameObject;
        private EnemyStateMachine ownerController;
        private float maxIddleTime;
        private float timer;

        public IddleState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerController = owner.GetComponent<EnemyStateMachine>();
            maxIddleTime = ownerController.EnemyParameters.MaxIddleTime;  
        }

        public void OnEnter()
        {
            timer = 0f;
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
            return (timer > maxIddleTime);
        }
    }
}
