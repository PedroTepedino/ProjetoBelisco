using Sirenix.OdinInspector;
using UnityEngine;

/* Class: PlayerGrounder
 *  Manages if the player is touching the ground or not
 *  
 * About: GroundCheck
 *  Sends a Signal and have a Propertie to indicate if the player is touching the ground or not.
 */
namespace GameScripts.Player
{
    public class Grounder : MonoBehaviour
    {
        /* Variables: 
     * _grounderCenter - Vector3 that describes the Center of the Grounder Check, relative to the center of the player
     * _grounderSizes - Vector2 that describes the Sizes of the grounder Box.
     * _grounderLayerMask - Layers that the grounder can interact, aka the ground.
     */
        [FoldoutGroup("Parameters")] [SerializeField] private Vector3 _grounderCenter = Vector3.zero;
        [FoldoutGroup("Parameters")] [SerializeField] private Vector2 _grounderSizes;
        [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;
        [FoldoutGroup("Parameters")] [SerializeField] private float _permitedArialTime = 0.005f;

        /* Variable: OnGrounded
     * Signal that sends info to the subscribed functions. 
     */
        public System.Action<bool> OnGrounded;
        public static System.Action OnTouchGround;

        /* Variables: Properties
     * IsGrounded - Propertie, that returns if the player is grounded or not.
     * ArialTime - Propertie that stores the time is seconds, that the player is not grounded.
     */
        public static bool IsGrounded { get; private set; } = false;
        public static float TotalArialTime { get; private set; } = 0f;
        public static bool IsTouchingGround { get; private set; } = false;
        public static bool IsWithinPermitedArialTime { get; private set; } = false;

        private void Awake()
        {
            Life.OnPlayerSpawn += ResetParameters;
        }

        private void OnDestroy()
        {
            Life.OnPlayerSpawn -= ResetParameters;
        }


        /* Function: Update
     * Unity update function, runs every frame, verifing if the player is touching the ground or not.
     */
        private void Update()
        {
            bool lastFrameIsTouchingGround = IsTouchingGround;
            IsTouchingGround = GroundCheck();
            JustTouchGround(lastFrameIsTouchingGround, IsTouchingGround);

            ArialTimeCalculation();
            IsGroundedVerification();
        }

        private void ResetParameters(GameObject obj)
        {
            IsGrounded = false;
            TotalArialTime = 0f;
            IsTouchingGround = false;
            IsWithinPermitedArialTime = false;
        }

        /* Function: IsGroundedVerification
     * Calls the logic of the Ground check every frame.
     */
        private void IsGroundedVerification()
        {
            IsWithinPermitedArialTime = ArialTimeCheck();

            if ((IsTouchingGround || IsWithinPermitedArialTime) == IsGrounded) return;
        
            IsGrounded = IsTouchingGround;
            OnGrounded?.Invoke(IsGrounded);
        }

        /* Function: ArialTimeCalculation
     * Calculates the Arial Time of the player every Frame.
     */
        private void ArialTimeCalculation()
        {
            if (IsTouchingGround)
            {
                TotalArialTime = 0f;
            }
            else
            {
                TotalArialTime += Time.deltaTime;
            }
        }

        /* Function: GroundCheck
     *  Checks if the player is touching the ground or not.
     * Returns:
     *  True if the player is touching the ground.
     */
        private bool GroundCheck()
        {
            if (PlatformDown.IsFallingThrough) return false;

            return Physics2D.OverlapBox(this.transform.position + _grounderCenter, _grounderSizes, 0f, _grounderLayerMask) != null ? true : false;
        }

        private void JustTouchGround(bool lastFrame, bool thisFrame)
        {
            if (thisFrame && !lastFrame)
            {
                OnTouchGround?.Invoke();
            }

        }

        private bool ArialTimeCheck()
        {
            return TotalArialTime < _permitedArialTime;
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
}
