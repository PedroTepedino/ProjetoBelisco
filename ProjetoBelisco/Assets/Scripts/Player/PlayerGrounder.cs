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
    [FoldoutGroup("Paremeters")] [SerializeField] private float _grounderRadius;
    [FoldoutGroup("Paremeters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;

    /* Variable: OnGrounded
     * Signal that sends info to the subscribed functions. 
     */
    public System.Action<bool> OnGrounded;

    /* Variable: IsGrounded
     * Propertie, that returns if the player is grounded or not.
     */
    public bool IsGrounded { get; private set; } = false;
    
    /* Function: Update
     * Unity update function, runs every frame, verifing if the player is touching the ground or not.
     */
    private void Update()
    {
        bool auxiliarIsGrounded = GroundCheck();

        if (auxiliarIsGrounded != IsGrounded)
        {
            IsGrounded = auxiliarIsGrounded;
            OnGrounded?.Invoke(IsGrounded);
        }
    }

    /* Function: GroundCheck
     *  Checks if the player is touching the ground or not.
     * Returns:
     *  True if the player is touching the ground.
     */
    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(this.transform.position + _grounderCenter, _grounderRadius, _grounderLayerMask) != null ? true : false;
    }

    /* Function: OnDrawGizmos
     * Unity function, that draws the gizmos on the Editor screen, to better visualize the grounder.
     */
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + _grounderCenter, _grounderRadius);
    }
}
