using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class VoidHole : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Kill(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Kill(collision);
    }

    private void Kill(Collider2D collision)
    {
        BaseLifeSystem lifeSystem = collision.gameObject.GetComponentInChildren<BaseLifeSystem>();

        if (lifeSystem != null)
        {
            lifeSystem.Damage(lifeSystem.CurentHealth);
        }
    }
}