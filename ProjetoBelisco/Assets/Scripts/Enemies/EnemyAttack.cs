using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyAttack : MonoBehaviour
{
    [FoldoutGroup("Parameters")] [SerializeField] private Vector2 attackPoint;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask collisionLayerMask;
    [FoldoutGroup("Parameters")] [SerializeField] public float attackRange { get; private set; } = 1f;
    [FoldoutGroup("Parameters")] [SerializeField] public float attackSpeed { get; private set; } = 1f;
    [FoldoutGroup("Parameters")] [SerializeField] private bool meleeAttack = false;
    [FoldoutGroup("Parameters")] [SerializeField] private bool explosionAttack = false;
    [FoldoutGroup("Parameters")] [SerializeField] private bool rangedAttack = false;

    [FoldoutGroup("Melee Attack Parameters")] [SerializeField] [Range(0, 100)] private int meleeAtttackChance = 100;
    [FoldoutGroup("Melee Attack Parameters")] [SerializeField] private float meleeAttackRadius = 1f;
    [FoldoutGroup("Melee Attack Parameters")] [SerializeField] private int meleeAttackDamage = 1;

    [FoldoutGroup("Melee Attack Parameters")] [SerializeField] [Range(0, 100)] private int explosionAtttackChance = 100;
    [FoldoutGroup("Explosion AttackParameters")] [SerializeField] private float explosionAttackRadius = 1f;
    [FoldoutGroup("Explosion Attack Parameters")] [SerializeField] private int explosionAttackDamage = 1;

    [FoldoutGroup("Melee Attack Parameters")] [SerializeField] [Range(0, 100)] private int rangedAtttackChance = 100;
    [FoldoutGroup("Range Attack Parameters")] [SerializeField] private int rangeAttackDamage = 1;
    [FoldoutGroup("Range Attack Parameters")] [SerializeField] private GameObject rangeAttackProjectile;

    private List<int> attacksChances;

    private void Start()
    {
        if (meleeAttack)
        {
            attacksChances.Add(0);
        }
        if (explosionAttack)
        {
            attacksChances.Add(1);
        }
        if (rangedAttack)
        {
            attacksChances.Add(2);
        }

    }

    public void Attack(Transform target)
    {
        if (target != null)
        {
            int choice = Random.Range(0, attacksChances.Count);
            if (attacksChances[choice] == 0)
            {
                MeleeAttack();
            }
            else if (attacksChances[choice] == 1)
            {
                ExplosionAttack();
            }
            else if (attacksChances[choice] == 2)
            {
                RangedAttack();
            }
        }
    }

    private void MeleeAttack()
    {
        Collider2D[] rayHits = Physics2D.OverlapCircleAll(attackPoint, meleeAttackRadius, collisionLayerMask);
        Collider2D hit = CheckHit(rayHits);
        if (hit != null)
        {
            //damage
            hit.gameObject.GetComponent<PlayerLife>().Damage(meleeAttackDamage);
        }
    }

    private void ExplosionAttack()
    {
        Collider2D[] rayHits = Physics2D.OverlapCircleAll(this.transform.position, explosionAttackRadius, collisionLayerMask);
        Collider2D hit = CheckHit(rayHits);
        if (hit != null)
        {
            //damage
            hit.gameObject.GetComponent<PlayerLife>().Damage(explosionAttackDamage);
        }
    }

    private void RangedAttack()
    {
        Instantiate(rangeAttackProjectile, attackPoint, this.transform.rotation);
    }

    private Collider2D CheckHit(Collider2D[] hits)
    {
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                RaycastHit2D raycastHit2D = Physics2D.Linecast(this.transform.position, hit.transform.position, collisionLayerMask);
                if (raycastHit2D.transform.gameObject == hit.gameObject)
                {
                    return hit;
                }
                else
                {
                    return null;
                }
            }
        }
        return null;
    }
}
