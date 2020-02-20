using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerMovement : MonoBehaviour
{
    //// Parameters
    [BoxGroup("Parameters")] [SerializeField] [FoldoutGroup("Parameters/Movement")] private float moveSpeed = 10f;

    private Rigidbody2D _rigidbody;

    //// Methods
    /// Unity Methods
    private void Awake()
    {
        GetEssentialComponents();
    }

    /// Inicialization
    private void GetEssentialComponents()
    {
        _rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    /// Movement
    public void MovePlayer(float direction)
    {
        Vector2 moveDirection = new Vector2(direction * moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = moveDirection;
    }
}
