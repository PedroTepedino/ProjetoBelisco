using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerGravityManager
 * Manages the gravity interaction of the player.
 */
public class PlayerGravityManager : MonoBehaviour
{
    // Group: Private Variables
    /* Variables: Parameters
     * _standardGravityMultiplier - Gravity multiplier for the normal situations.
     * _fallingGravityMultiplier - The multiplier for the gravity when the player is falling.
     */
    [SerializeField] [FoldoutGroup("Parameters")] private float _standardGravityMultiplier = 1f;
    [SerializeField] [FoldoutGroup("Parameters")] private float _fallingGravityMultiplier = 3f;

    /* Variables: Essential Components
     * _rigidBody - The RigidBody of the player.
     * _playerGrounder - Stores the player <PlayerGrounder> component.
     * _playerJump - Stores the player <PlayerJump> component.
     */
    private Rigidbody2D _rigidBody;
    private PlayerGrounder _playerGrounder;

    // Group: Unity Methods
    /* Function: Awake
     * Calls the <Setup Methods>.
     */
    private void Awake()
    {
        GetEssentialComponents();
    }

    /* Function: Update
     * Calls the <Gravity Logic> methods that need to be called every frame.
     */
    private void Update()
    {
        GravityDecision(_playerGrounder.IsGrounded); 
    }

    // Group: Setup Methods
    /* Function: GetEssentialComponents
     * Retrives the <Essential Components> from the player.
     */
    private void GetEssentialComponents()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _playerGrounder = this.GetComponent<PlayerGrounder>();
    }

    // Group: Gravity Logic
    /* Function: GravityDecision.
     *  Decides witch gravity value to apply every frame.
     *  
     * Parameters: 
     *  isGrounded - Boolean that recieves the info if the player is grounded or not.
     */
    private void GravityDecision(bool isGrounded)
    {
        if (isGrounded)
        {
            ResetGravity();
        }
        else
        {
            if (_rigidBody.velocity.y < 0f)
            {
                FallingGravity();
            }
        }
    }

    /* Function: ResetGravity
     * Resets the gravity to the <standard value: Parameters>.
     */
    private void ResetGravity()
    {
        _rigidBody.gravityScale = _standardGravityMultiplier;
    }

    /* Function: FallingGravity
     * Sets the gravity to the <falling value: Parameters>.
     */
    private void FallingGravity()
    {
        _rigidBody.gravityScale = _fallingGravityMultiplier;
    }
}
