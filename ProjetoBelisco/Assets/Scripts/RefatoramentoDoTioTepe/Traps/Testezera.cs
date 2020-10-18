using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Testezera : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        [SerializeField] private float _forceMultiplier = 0f;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Debug.Log(Math.Round(_rigidbody.velocity.magnitude, 2));
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(Vector2.right * _forceMultiplier);
        }
    }
}