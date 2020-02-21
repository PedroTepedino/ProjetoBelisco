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
     */
    [BoxGroup("Parameters")] [SerializeField] [FoldoutGroup("Parameters/Movement")] private float _moveSpeed = 10f;

    /* Variable: _rigidBody
     *  Stores the RigidBody of the player.
     */
    private Rigidbody2D _rigidBody;

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
    public void MovePlayer(float direction)
    {
        Vector2 moveDirection = new Vector2(direction * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = moveDirection;
    }
}
