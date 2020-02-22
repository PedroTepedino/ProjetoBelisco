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
     */
    private Rigidbody2D _rigidBody;

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
        GravityDecision(PlayerGrounder.IsGrounded); 
    }

    /* Function: OnDestroy
     * Handles the class right before the destruction of the GameObject, by calling the <Destruction Methods>
     */
    private void OnDestroy()
    {
        UnsubscribeFunctins();
    }

    // Group: Setup Methods
    /* Function: GetEssentialComponents
     * Retrives the <Essential Components> from the player.
     */
    private void GetEssentialComponents()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    /* Function: SubscribeFunctions
     * Subscribe the <Listeners> Methods to their respective signals
     */
    private void SubscribeFunctions()
    {
        PlayerJump.OnJump += OnJumpListener;
    }

    // Group: Destruction Methods
    /* Function: UnsubscribeFunctins
     * Remove the subscription of the <Listeners>, subscribed at <SubscribeFunctions>.
     */
    private void UnsubscribeFunctins()
    {
        PlayerJump.OnJump -= OnJumpListener;
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

    // Group: Listeners
    // Listens to certein function callings
    private void OnJumpListener(bool isJump)
    {
        ResetGravity();
    }
}
