using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerJump
 * Handles the jump mechanic of the player.
 */
public class PlayerJump : MonoBehaviour
{
    // Group: Private Variables
    /* Variables: Parameters
     * _jumpInitialVelocity - The initial jump velocity value.
     */
    [SerializeField] [FoldoutGroup("Parameters")] private float _jumpInitialVelocity = 10f; 

    /* Variables: Essential Components
     * _rigidBody - The RigidBody of the player.
     * _playerGrounder - Stores the player Grounder component.
     */
    private Rigidbody2D _rigidBody;
    private PlayerGrounder _playerGrounder;

    // Group: Public Variables
    /* Variable: OnJump
     * Sends a signal when start jumping and on stop
     */
    public System.Action<bool> OnJump;

    // Group: Properties
    /* Properties: Helper Properties
     * 
     * About::
     *  Properties that help the player to acess certein informations faster.
     * 
     * Vars::
     * IsJumping - Returns if the player is jumping or not.
     */
    public bool IsJumping { get; private set; } = false;

    // Group: Unity Methods
    /* Function: Awake
     * Calls the <Setup Methods>.
     */
    private void Awake()
    {
        GetEssentialComponents();
        SubscribeFunctions();
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
        _playerGrounder = this.GetComponent<PlayerGrounder>();
    }

    /* Function: SubscribeFunctions
     * Subscribe the <Listeners> Methods to their respective signals
     */
    private void SubscribeFunctions()
    {
        _playerGrounder.OnGrounded += ListenIsGrounded;
    }

    // Group: Destruction Methods
    /* Function: UnsubscribeFunctins
     * Remove the subscription of the <Listeners>, subscribed at <SubscribeFunctions>.
     */
    private void UnsubscribeFunctins()
    {
        _playerGrounder.OnGrounded -= ListenIsGrounded;
    }

    // Group: Jump Logic
    /* Function: Jump
     * the Jumping Action of the player
     */
    public void Jump()
    {
        this._rigidBody.velocity = new Vector2(this._rigidBody.velocity.x, _jumpInitialVelocity);
        IsJumping = true;
        OnJump?.Invoke(true);
    }


    /* Function: EndJump
     * Ends the Jumping Action
     */
    private void EndJump()
    {
        IsJumping = false;
        OnJump?.Invoke(false);
    }

    /* Function: CanJump
     * Decides if the player can jump or not.
     * Returns:
     *  True if the player can jump at a given frame.
     */
    public bool CanJump()
    {
        if (!IsJumping && _playerGrounder.IsGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Group: Listeners
    /* Function: ListenIsGrounded
     *  Listen to the grounder action.
     * 
     * Parameters: 
     *  isGrounded - Boolean that recieves the info if the player is grounded or not.
     */
    private void ListenIsGrounded(bool isGrounded)
    {
        if (IsJumping)
        {
            EndJump();
        }
    }
}
