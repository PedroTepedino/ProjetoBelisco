using UnityEngine;

namespace GameScripts.Player
{
    public class FrictionController : MonoBehaviour
    {
        [SerializeField] private PhysicsMaterial2D _materialGround;
        [SerializeField] private PhysicsMaterial2D _materialFall;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody.sharedMaterial = Grounder.IsTouchingGround ? _materialGround : _materialFall;
        }
    }
}
