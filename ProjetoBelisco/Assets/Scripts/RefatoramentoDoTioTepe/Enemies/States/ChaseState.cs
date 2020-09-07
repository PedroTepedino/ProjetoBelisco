using GameScripts.Enemies;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class ChaseState : IState
    {
        private GameObject ownerGameObject;
        private IEnemyStateMachine ownerController;
        private Targeting targeting;
        private Rigidbody2D ownerRigidbody;
        private GrounderEnemy grounder;
        private WallChecker wallCheck;
        private Transform target;
        private Vector2 movement;
        private float movingSpeed;

        public ChaseState(GameObject owner){
            ownerGameObject = owner;
            ownerController = owner.GetComponent<IEnemyStateMachine>();
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            grounder = ownerGameObject.GetComponent<GrounderEnemy>();
            wallCheck = ownerGameObject.GetComponent<WallChecker>();
            targeting = ownerGameObject.GetComponent<Targeting>();
            movingSpeed = ownerController.EnemyParameters.MovingSpeed;
        }
        public void OnEnter(){
            target = targeting.target;          
        }

        public void Tick()
        {
            target = targeting.target;

            if (target != null)
            {
                Vector2 direction = (target.position - ownerGameObject.transform.position);

                Move(direction);
                
            }
        }

        public void OnExit()
        {
        
        }

        private void Move(Vector2 direction)
        {
            if (grounder.isGrounded && !wallCheck.wallAhead)
            {
                ownerController.movingRight = ((direction.x > 0f) ? true : false);
                movement.Set(ownerController.movingRight ? movingSpeed : -movingSpeed, ownerRigidbody.velocity.y);
                ownerRigidbody.velocity = movement;
            }
            else
            {
                Flip();
            }
        }

        private void Flip()
        {
            ownerGameObject.transform.eulerAngles = new Vector3(0f, ownerController.movingRight ? -180f : 0f, 0f);
            ownerController.movingRight = ownerController.movingRight ? false : true;
        }
    }
}
