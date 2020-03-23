using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinValidator;

public class PlayerAttackSystem : BaseAttackSystem
{
    [System.Serializable]
    private struct AttackDirection
    {
        public Vector2 AttackCenter;
        
        [MinValue(0f)]
        public float Radius;

        public AttackDirection(Vector2 attackCenter, float radius = 1f )
        {
            AttackCenter = attackCenter;
            Radius = radius;
        }
    }
    
    [SerializeField] [EnumToggleButtons] private LayerMask _attackLayers;
    [SerializeField] [EnumToggleButtons] private LayerMask _wallCheckLayers;

    [SerializeField] private Dictionary<Directions, AttackDirection> _attackCenterDictionary;
    
    public static System.Action OnDamage;

    public override void Attack()
    {
        Collider2D[] enemies = CheckCollision();

        foreach (Collider2D coll in enemies)
        {
            BaseLifeSystem life = CheckWallInBetween(coll);

            if (life == null) continue;
            
            OnDamage?.Invoke();
            life.Damage(_baseAttack);
        }
    }

    public void Attack(Directions dir)
    {
        Collider2D[] enemies = CheckCollision(dir);

        foreach (Collider2D coll in enemies)
        {
            BaseLifeSystem life = CheckWallInBetween(coll);

            if (life == null) continue;
            
            OnDamage?.Invoke();
            life.Damage(_baseAttack);
        }
    }

    private Collider2D[] CheckCollision()
    {
        AttackDirection aux = _attackCenterDictionary[Directions.Right];
        if (!PlayerMovement.IsLookingRight)
        {
            aux = _attackCenterDictionary[Directions.Left];
        }
        
        return Physics2D.OverlapCircleAll((Vector2) this.transform.position + aux.AttackCenter, aux.Radius,
            _attackLayers);
    }

    private Collider2D[] CheckCollision(Directions dir)
    {
        if (dir == Directions.Null) return CheckCollision();

        AttackDirection aux = _attackCenterDictionary[dir];
        
        return Physics2D.OverlapCircleAll((Vector2) this.transform.position + aux.AttackCenter, aux.Radius,
            _attackLayers);
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
        Gizmos.color = Color.green;

        foreach (var pair in _attackCenterDictionary)
        {
            Gizmos.DrawWireSphere((Vector2)this.transform.position + pair.Value.AttackCenter, pair.Value.Radius);    
        }
    }
}
