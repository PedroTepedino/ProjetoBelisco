using UnityEngine;
using Sirenix.OdinInspector;

/* Class: PlayerGrounder
 * Manages if the player is touching the ground or not
 * About: 
 */
public class PlayerGrounder : MonoBehaviour
{
    [FoldoutGroup("Paremeters")] [SerializeField] private Vector3 _groundeCenter = Vector3.zero;
    [FoldoutGroup("Paremeters")] [SerializeField] private Vector2 _grounderSizes;
    [FoldoutGroup("Paremeters")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;

    public bool IsGrounded { get; private set; } = false;

    public System.Action<bool> OnGrounded;

    private void Update()
    {
        bool auxiliarIsGrounded = GroundCheck();

        if (auxiliarIsGrounded != IsGrounded)
        {
            IsGrounded = auxiliarIsGrounded;
            OnGrounded?.Invoke(IsGrounded);
        }
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapBox(this.transform.position + _groundeCenter, _grounderSizes, 0f, _grounderLayerMask);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(this.transform.position + _groundeCenter, _grounderSizes);
    }
}
