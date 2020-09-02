using GameScripts.Enemies;
using UnityEngine;

/* Class: MoveState
 * Handle movement of the owner of this state.
 */
namespace RefatoramentoDoTioTepe
{
    public class MoveState : IState
    {
        // Group: Private Variables

        /* Variables:
     * ownerGameObject - GameObject of the enemy controling this state.
     * controllerOwner - <EnemyController> or a function that inherits of the enemy controling this state.
     * movingRight - Boolean controlling the direction of the movement.
     * groundCheck - Boolean that check if has ground ahead.
     * wallCheck - Boolean that check if has an obtacle ahead.
     * movement - Vector2 that controls the magnitude of the movement
     */
        private GameObject ownerGameObject;
        private EnemyStateMachine ownerController;
        private Rigidbody2D ownerRigidbody;
        private GrounderEnemy grounder;
        private WallChecker wallCheck;
        private Vector2 movement = new Vector2();
        private bool groundCheck;
        private float timer;
        private float timerBetweeneStops;
        private float timeToWait;
        private float maxStopTime;
        private float minTimeBetweenStops;
        private float movingSpeed;

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
            ownerController = gameObject.GetComponent<EnemyStateMachine>();
            ownerRigidbody = ownerGameObject.GetComponent<Rigidbody2D>();
            grounder = ownerGameObject.GetComponent<GrounderEnemy>();
            wallCheck = ownerGameObject.GetComponent<WallChecker>();
            maxStopTime = ownerController.EnemyParameters.MaxStopTime;
            minTimeBetweenStops = ownerController.EnemyParameters.MinTimeBetweenStops;
            movingSpeed = ownerController.EnemyParameters.MovingSpeed;
        }

        /*Function: EnterState
     * Commands executed when enter the state.
     */
        public void OnEnter()
        {
            timeToWait = 0;
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
            timerBetweeneStops += Time.deltaTime;
            int chanceToStop;
            chanceToStop = Random.Range(0,10);

            if (grounder.isGrounded && !wallCheck.wallAhead)
            {
                if (timer >= timeToWait)
                {
                    Move();
                }
                else
                {
                    if (timerBetweeneStops > minTimeBetweenStops)
                    {
                        Stop();
                    }
                }
            }
            else
            {
                Stop();
                Flip();
            }
        }

        private void Stop()
        {
            ownerController.rigidbody.velocity = Vector2.zero;
            timerBetweeneStops = 0;
        }
        private void Move()
        {
            movement.Set(ownerController.movingRight ? movingSpeed : -movingSpeed, ownerRigidbody.velocity.y);
            ownerRigidbody.velocity = movement;
            timeToWait = Random.Range(0, maxStopTime);
            timer = 0;
        }

        private void Flip()
        {
            ownerGameObject.transform.eulerAngles = new Vector3(0, ownerController.movingRight ? -180 : 0, 0);
            ownerController.movingRight = ownerController.movingRight ? false : true;
        }
        /* See Also:
     */
    }
}
