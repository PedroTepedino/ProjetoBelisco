using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Testezera : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(-Physics2D.gravity);
        }
    }
}