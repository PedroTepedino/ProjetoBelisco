using UnityEngine;

/* Class: MoveState
 * Handle movement of the owner of this state.
 */
namespace Belisco
{
    public class MoveState : IState
    {
        private readonly GrounderEnemy grounder;
        private readonly float maxTimeBetweenIddles;
        private readonly float minTimeBetweenIddles;
        private Vector2 movement;
        private readonly float movingSpeed;

        private readonly IEnemyStateMachine ownerController;
        // Group: Private Variables

        /* Variables:
     * ownerGameObject - GameObject of the enemy controling this state.
     * controllerOwner - <EnemyController> or a function that inherits of the enemy controling this state.
     * movingRight - Boolean controlling the direction of the movement.
     * groundCheck - Boolean that check if has ground ahead.
     * wallCheck - Boolean that check if has an obtacle ahead.
     * movement - Vector2 that controls the magnitude of the movement
     */
        private readonly GameObject ownerGameObject;
        private readonly Rigidbody2D ownerRigidbody;
        private float timer;
        private float timerToIddle;
        private readonly WallChecker wallCheck;

        // Group: Functions

        /* Constructor: MoveState
     * Initialize this state.
     * Parameters:
     * owner - GameObject of the enemy controling this state.
     * controller - <EnemyController> or a function that inherits of the enemy controling this state.
     */

        public MoveState(GameObject gameObject)
        {
            ownerGameObject = gameObject;
            ownerController = gameObject.GetComponent<IEnemyStateMachine>();
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            grounder = ownerGameObject.GetComponent<GrounderEnemy>();
            wallCheck = ownerGameObject.GetComponent<WallChecker>();
            minTimeBetweenIddles = ownerController.EnemyParameters.MinTimeBetweenIddles;
            maxTimeBetweenIddles = ownerController.EnemyParameters.MaxTimeBetweenIddles;
            movingSpeed = ownerController.EnemyParameters.MovingSpeed;
        }

        /*Function: EnterState
     * Commands executed when enter the state.
     */
        public void OnEnter()
        {
            timerToIddle = Random.Range(minTimeBetweenIddles, maxTimeBetweenIddles);
            timer = 0;
        }

        /*Function: ExitState
     * Commands executed before exit the state.
     */
        public void OnExit()
        {
        }

        /*Function: RunState
     * Moves the owner to the same direction until he finds an obstacle or the floor ends, when one of those happen he start to go the opposite direction.
     */
        public void Tick()
        {
            timer += Time.deltaTime;

            if (grounder.isGrounded && !wallCheck.wallAhead)
                Move();
            else
                Flip();
        }


        private void Move()
        {
            movement.Set(ownerController.movingRight ? movingSpeed : -movingSpeed, ownerRigidbody.velocity.y);
            ownerRigidbody.velocity = movement;
        }

        private void Flip()
        {
            ownerGameObject.transform.eulerAngles = new Vector3(0, ownerController.movingRight ? -180 : 0, 0);
            ownerController.movingRight = ownerController.movingRight ? false : true;
            timer = timerToIddle + 1f;
        }

        public bool TimeEnded()
        {
            return timer > timerToIddle;
        }

        /* See Also:
     */
    }
}