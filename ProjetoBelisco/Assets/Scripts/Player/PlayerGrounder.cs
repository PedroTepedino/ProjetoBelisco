using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerGrounder
 *  Manages if the player is touching the ground or not
 *  
 * About: GroundCheck
 *  Sends a Signal and have a Propertie to indicate if the player is touching the ground or not.
 */
public class PlayerGrounder : MonoBehaviour
{
    /* Variables: 
     * _grounderCenter - Vector3 that describes the Center of the Grounder Check, relative to the center of the player
     * _grounderSizes - Vector2 that describes the Sizes of the grounder Box.
     * _grounderLayerMask - Layers that the grounder can interact, aka the ground.
     */
    [FoldoutGroup("Paremeters")] [SerializeField] private Vector3 _grounderCenter = Vector3.zero;
    [FoldoutGroup("Paremeters")] [SerializeField] private Vector2 _grounderSizes;
    [FoldoutGroup("Paremeters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;

    /* Variable: OnGrounded
     * Signal that sends info to the subscribed functions. 
     */
    public System.Action<bool> OnGrounded;

    /* Variables: Properties
     * IsGrounded - Propertie, that returns if the player is grounded or not.
     * ArialTime - Propertie that stores the time is seconds, that the player is not grounded.
     */
    public static bool IsGrounded { get; private set; } = false;
    public static float ArialTime { get; private set; } = 0f;
    
    /* Function: Update
     * Unity update function, runs every frame, verifing if the player is touching the ground or not.
     */
    private void Update()
    {
        IsGroundedVerification();
        ArialTimeCalculation();
    }

    /* Function: IsGroundedVerification
     * Calls the logic of the Ground check every frame.
     */
    private void IsGroundedVerification()
    {
        bool auxiliarIsGrounded = GroundCheck();

        if (auxiliarIsGrounded != IsGrounded)
        {
            IsGrounded = auxiliarIsGrounded;
            OnGrounded?.Invoke(IsGrounded);
        }
    }

    /* Function: ArialTimeCalculation
     * Calculates the Arial Time of the player every Frame.
     */
    private void ArialTimeCalculation()
    {
        if (IsGrounded)
        {
            ArialTime = 0f;
        }
        else
        {
            ArialTime += Time.deltaTime;
        }
    }

    /* Function: GroundCheck
     *  Checks if the player is touching the ground or not.
     * Returns:
     *  True if the player is touching the ground.
     */
    private bool GroundCheck()
    {
        return Physics2D.OverlapBox(this.transform.position + _grounderCenter, _grounderSizes, 0f, _grounderLayerMask) != null ? true : false;
    }

    /* Function: OnDrawGizmos
     * Unity function, that draws the gizmos on the Editor screen, to better visualize the grounder.
     */
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + _grounderCenter, _grounderSizes);
    }
}
