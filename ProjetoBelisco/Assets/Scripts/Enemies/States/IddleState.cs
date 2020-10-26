using UnityEngine;

namespace Belisco
{
    public class IddleState : IState
    {
        private float iddleTime;
        private readonly float maxIddleTime;
        private float minIddleTime;
        private readonly IEnemyStateMachine ownerController;
        private readonly GameObject ownerGameObject;
        private readonly Rigidbody2D ownerRigidbody;
        private float timer;

        public IddleState(GameObject owner)
        {
            ownerGameObject = owner;
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            ownerController = owner.GetComponent<IEnemyStateMachine>();
            maxIddleTime = ownerController.EnemyParameters.MaxIddleTime;
            maxIddleTime = ownerController.EnemyParameters.MinIddleTime;
        }

        public void OnEnter()
        {
            ownerRigidbody.velocity = Vector2.zero;
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
            return timer > iddleTime;
        }
    }
}