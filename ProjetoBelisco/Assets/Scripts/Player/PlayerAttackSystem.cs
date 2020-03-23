using System;
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

    private Directions _currentAttackDirection = Directions.Null;

    private PlayerJump _playerJump;
    
    public static System.Action OnDamage;

    private void Awake()
    {
        _playerJump = this.GetComponent<PlayerJump>();
    }

    public override void Attack()
    {
        Attack(Directions.Null);
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

        if (enemies.Length > 0 && _currentAttackDirection == Directions.Down)
        {
            _playerJump.Jump();
            
        }
    }

    private Collider2D[] CheckCollision()
    {
        _currentAttackDirection = Directions.Right;
        
        if (!PlayerMovement.IsLookingRight)
        {
            _currentAttackDirection = Directions.Left;
        }
        
        AttackDirection aux = _attackCenterDictionary[_currentAttackDirection];

        return Physics2D.OverlapCircleAll((Vector2) this.transform.position + aux.AttackCenter, aux.Radius,
            _attackLayers);
    }

    private Collider2D[] CheckCollision(Directions dir)
    {
        if (dir == Directions.Null) return CheckCollision();

        _currentAttackDirection = dir;    
        
        AttackDirection aux = _attackCenterDictionary[_currentAttackDirection];
        
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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (var pair in _attackCenterDictionary)
        {
            Gizmos.color = pair.Key == _currentAttackDirection ? Color.red : Color.black;

            Gizmos.DrawWireSphere((Vector2)this.transform.position + pair.Value.AttackCenter, pair.Value.Radius);
        }
    }
#endif
}
