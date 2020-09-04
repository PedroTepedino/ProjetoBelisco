using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class IddleState : IState
    {
        private GameObject ownerGameObject;
        private IEnemyStateMachine ownerController;
        private float maxIddleTime;
        private float minIddleTime;
        private float iddleTime;
        private float timer;

        public IddleState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerController = owner.GetComponent<IEnemyStateMachine>();
            maxIddleTime = ownerController.EnemyParameters.MaxIddleTime; 
            maxIddleTime = ownerController.EnemyParameters.MinIddleTime; 
        }

        public void OnEnter()
        {
            timer = 0f;
            iddleTime = Random.Range(minIddleTime, maxIddleTime);
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
