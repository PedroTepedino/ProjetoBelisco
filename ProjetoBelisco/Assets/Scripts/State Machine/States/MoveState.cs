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
    private RaycastHit2D groundCheck;
    private RaycastHit2D wallCheck;
    private Vector2 movement;

    // Group: Functions

    /* Constructor: MoveState
     * Initialize this state.
     * Parameters:
     * owner - GameObject of the enemy controling this state.
     * controller - <EnemyController> or a function that inherits of the enemy controling this state.
     */

    public MoveState(GameObject gameObject, EnemyController controller)
    {
        ownerGameObject = gameObject;
        controllerOwner = controller;
    }

    /*Function: EnterState
     * Commands executed when enter the state.
     */
    public void EnterState()
    {
        controllerOwner.actualState = "move";
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
        groundCheck = Physics2D.Raycast(controllerOwner.groundDetection.position, Vector2.down, 2f, controllerOwner.layerMove);
        if (groundCheck.collider != null)
        {
            Debug.Log(groundCheck.collider.gameObject);
        }
        else
        {
            Debug.Log("null ground");
        }
        if (controllerOwner.movingRight)
        {
            wallCheck = Physics2D.Raycast(controllerOwner.groundDetection.position, Vector2.right, 0.5f, controllerOwner.layerMove);
        }
        else
        {
            wallCheck = Physics2D.Raycast(controllerOwner.groundDetection.position, Vector2.left, 0.5f, controllerOwner.layerMove);
        }
        
        if (wallCheck.collider != null)
        {
            Debug.Log(wallCheck.collider.gameObject);
        }
        else
        {
            Debug.Log("null wall");
        }
        if (groundCheck.collider != null  && wallCheck.collider == null)
        {
            if (controllerOwner.movingRight)
            {
                movement.Set(controllerOwner.movingSpeed * Time.deltaTime, controllerOwner.rigidbody.velocity.y);
                controllerOwner.rigidbody.velocity = movement;
            }
            else
            {
                movement.Set(-controllerOwner.movingSpeed * Time.deltaTime, controllerOwner.rigidbody.velocity.y);
                controllerOwner.rigidbody.velocity = movement;
            }
           
        }
        else
        {
            if (controllerOwner.movingRight)
            {
                ownerGameObject.transform.eulerAngles = new Vector3(0, -180, 0);
                controllerOwner.movingRight = false;
            }
            else
            {
                ownerGameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                controllerOwner.movingRight = true;
            }
        }
    }
    /* See Also:
     */
}
