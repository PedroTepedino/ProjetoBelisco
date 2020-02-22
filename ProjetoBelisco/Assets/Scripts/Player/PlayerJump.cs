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
     * _jumpActionMaxHoldingTime - Maximum time in witch the player can hold the jump button and still ascend.
     * _permitedArialTime - Time that the player can still jump if falling from a platform.
     */
    [SerializeField] [FoldoutGroup("Parameters")] private float _jumpInitialVelocity = 10f;
    [SerializeField] [FoldoutGroup("Parameters")] private float _jumpActionMaxHoldingTime = 1.5f;
    [SerializeField] [FoldoutGroup("Parameters")] private float _permitedArialTime = 0.2f;

    /* Variables: Essential Components
     * _rigidBody - The RigidBody of the player.
     */
    private Rigidbody2D _rigidBody;

    // Group: Public Variables
    /* Variable: OnJump
     * Sends a signal when start jumping and on stop
     */
    public static System.Action<bool> OnJump;

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
    }

    // Group: Setup Methods
    /* Function: GetEssentialComponents
     * Retrives the <Essential Components> from the player.
     */
    private void GetEssentialComponents()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
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
        if(PlayerGrounder.IsGrounded)
        {
            EndJump();
        }

        if (!IsJumping)
        {
            if (PlayerGrounder.ArialTime <= _permitedArialTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /* Function: CanRaiseJump
     *  Determins if the player can raise the jump already started.
     *  Parameters:
     *  jumpTime - The time in seconds that the player has held down the jump button.
     *  Returns:
     *  True if the player can follw the jump.
     */
    public bool CanRaiseJump(float jumpTime)
    {
        if (IsJumping && (jumpTime <= _jumpActionMaxHoldingTime))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
