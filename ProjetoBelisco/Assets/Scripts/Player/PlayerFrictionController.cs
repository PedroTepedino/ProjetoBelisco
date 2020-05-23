using System;
using UnityEngine;

public class PlayerFrictionController : MonoBehaviour
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
        _rigidbody.sharedMaterial = PlayerGrounder.IsTouchingGround ? _materialGround : _materialFall;
    }
}
