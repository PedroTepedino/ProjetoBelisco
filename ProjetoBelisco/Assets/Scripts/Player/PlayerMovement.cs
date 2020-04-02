using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerMovement
 *  Resolves the Movement of the player
 */
public class PlayerMovement : MonoBehaviour
{
    // Group: Private Variables

    /* Floats: 
     * _moveSpeed - Stores the Movement Speed used by the player.
     * _accelerationTime - Time that takes the player to accelerato to max speed.
     * _decelerationTime - Time that takes the player to come to full stop.
     * _timeToHalt - Time the player needs to be considered not inputing a moving action.
     * _minimumVelocity - The utmost minimum velocity the player can have.
     */
    [BoxGroup("Parameters")] [SerializeField] private float _moveSpeed = 10f;
    [BoxGroup("Parameters")] [SerializeField] private float _accelerationTime = 0.1f;
    [BoxGroup("Parameters")] [SerializeField] private float _decelerationTime = 0.05f;
    [BoxGroup("Parameters")] [SerializeField] private float _timeToHalt = 0.2f;
    [BoxGroup("Parameters")] [SerializeField] private float _minimumVelocity = 0.25f;

    /* Variable: _rigidBody
     *  Stores the RigidBody of the player.
     */
    private Rigidbody2D _rigidBody;

    // Group: Properties
    /* Variables:
     * IsMoving - Determins if the player is moving or came to a full stop.
     */
    public bool IsMoving { get; private set; } = false;

    // Group: Unity Methods
    /* Function: Awake
     *  Calls the <Setup Methods>.
     */
    private void Awake()
    {
        GetEssentialComponents();
    }

    // Group: Setup Methods
    /* Function: GetEssentialComponents
     * Setup the essential components necessery to the class.
     */
    private void GetEssentialComponents()
    {
        _rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Group: Movement Logic
    /* Function: MovePlayer
     * Moves the player Left and Right.
     * Parameters :
     * direction - the direction in witch the pllayer should go 1 rigth, -1 left.
     */
    public void MovePlayer(float direction, float movementTime)
    {
        direction = (direction > 0 ? 1 : -1);
        
        if (!IsMoving)
        {
            if (movementTime < _accelerationTime)
            {
                direction *= movementTime / _accelerationTime;
            }
            else
            {
                IsMoving = true;
            }
        }
        
        Vector2 moveDirection = new Vector2(direction * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = moveDirection;
    }

    public void StopMovement(float stopMovementTime)
    {
        if (stopMovementTime > _timeToHalt)
        {
            IsMoving = false;
        }

        Vector2 haltVector;
        if (stopMovementTime <= _decelerationTime)
        {
            haltVector = new Vector2(_rigidBody.velocity.x * (stopMovementTime / _decelerationTime), _rigidBody.velocity.y);
        }
        else
        {
            haltVector = new Vector2(0, _rigidBody.velocity.y);
        }
        _rigidBody.velocity = haltVector;
    }
}
