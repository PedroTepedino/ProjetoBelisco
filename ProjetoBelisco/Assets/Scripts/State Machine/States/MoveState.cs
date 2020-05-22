using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Class: MoveState
 * Handle movement of the owner of this state.
 */
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
    private EnemyController controllerOwner;
    private EnemyGrounder grounder;
    private EnemyWallChecker wallCheck;
    private Vector2 movement;
    private bool groundCheck;
    private float timer;
    private float timerBetweeneStops;
    private float timeToWait;
    private float maxStopTime;
    private float minTimeBetweenStops;
     

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
        controllerOwner = gameObject.GetComponent<EnemyController>();
    }

    /*Function: EnterState
     * Commands executed when enter the state.
     */
    public void EnterState()
    {
        controllerOwner.actualState = "move";
        grounder = ownerGameObject.GetComponent<EnemyGrounder>();
        wallCheck = ownerGameObject.GetComponent<EnemyWallChecker>();
        maxStopTime = controllerOwner.maxStopTime;
        minTimeBetweenStops = controllerOwner.minTimeBetweenStops;
        timeToWait = 0;
        timer = 0;
    }

    /*Function: ExitState
     * Commands executed before exit the state.
     */
    public void ExitState()
    {

    }

    /*Function: RunState
     * Moves the owner to the same direction until he finds an obstacle or the floor ends, when one of those happen he start to go the opposite direction.
     */
    public void RunState()
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
        controllerOwner.rigidbody.velocity = Vector2.zero;
        timerBetweeneStops = 0;
    }
    private void Move()
    {
        movement.Set(controllerOwner.movingRight ? controllerOwner.movingSpeed : -controllerOwner.movingSpeed, controllerOwner.rigidbody.velocity.y);
        controllerOwner.rigidbody.velocity = movement;
        timeToWait = Random.Range(0, maxStopTime);
        timer = 0;
        Debug.Log(timeToWait);
    }

    private void Flip()
    {
        ownerGameObject.transform.eulerAngles = new Vector3(0, controllerOwner.movingRight ? -180 : 0, 0);
        controllerOwner.movingRight = controllerOwner.movingRight ? false : true;
    }
    /* See Also:
     */
}
