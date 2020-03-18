using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerAttackSystem : BaseAttackSystem
{
    [SerializeField] [FoldoutGroup("Parameters")] private float _attackAreaRadius = 1.0f;
    [SerializeField] [FoldoutGroup("Parameters")] private Vector3 _attackCenter = new Vector3(1.0f, 0f, 0f);
    [SerializeField] [EnumToggleButtons] private LayerMask _attackLayers;
    [SerializeField] [EnumToggleButtons] private LayerMask _wallCheckLayers;

    public static System.Action OnDamage;

    public override void Attack()
    {
        Collider2D[] enemies = CheckCollision();

        foreach (Collider2D collider in enemies)
        {
            BaseLifeSystem life = CheckWallInBetween(collider);
            if (life != null)
            {
                OnDamage?.Invoke();
                life.Damage(_baseAttack);
            }
        }
    }

    private Collider2D[] CheckCollision()
    {
        Vector3 aux = _attackCenter;
        if (!PlayerMovement.IsLookingRight)
        {
            aux.x = aux.x * -1f;
        }
        return Physics2D.OverlapCircleAll(this.transform.position + aux, _attackAreaRadius, _attackLayers);
    }

    private BaseLifeSystem CheckWallInBetween(Collider2D collider)
    {
        Vector2 auxVector = collider.transform.position - this.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, (auxVector).normalized, auxVector.magnitude, _wallCheckLayers | _attackLayers);

        if (hit.collider != null)
        {
            if (hit.collider == collider)
            {
                return hit.collider.gameObject.GetComponent<BaseLifeSystem>();
            }
        }
        else
        {
            return collider.gameObject.GetComponent<BaseLifeSystem>();
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Vector3 aux = _attackCenter;
        if (!PlayerMovement.IsLookingRight)
        {
            aux.x = aux.x * -1f;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + aux, _attackAreaRadius);
    }
}
