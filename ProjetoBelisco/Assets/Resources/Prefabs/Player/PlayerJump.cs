using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //// Parameters
    private Rigidbody2D _rigidBody;
    private PlayerGrounder _grounder;

    //// Methods
    /// Unity Methods
    private void Awake()
    {
        GetEssencialComponents();
    }

    /// Setup
    private void GetEssencialComponents()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _grounder = this.GetComponent<PlayerGrounder>();
    }

    /// Jump Logic
    public void Jump()
    {

    }

    public void Ascend()
    {

    }

    public void Descend()
    {

    }
}
