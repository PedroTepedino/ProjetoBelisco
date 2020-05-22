﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnemyAttack : MonoBehaviour
{
    [FoldoutGroup("Parameters")] [SerializeField] private Vector2 attackPoint;
    [FoldoutGroup("Parameters")] [SerializeField] [EnumToggleButtons] private LayerMask collisionLayerMask;
    [FoldoutGroup("Parameters")] [SerializeField] public float attackRange = 1f;
    [FoldoutGroup("Parameters")] [SerializeField] public float attackSpeed = 1f;

    [FoldoutGroup("Attacks Parameters")] [SerializeField] private bool meleeAttack = false;
    [FoldoutGroup("Attacks Parameters")] [SerializeField] private bool explosionAttack = false;
    [FoldoutGroup("Attacks Parameters")] [SerializeField] private bool rangedAttack = false;

    [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")] [SerializeField] [Range(1, 100)] private int meleeAtttackChance = 0;
    [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")] [SerializeField] private float meleeAttackRadius = 1f;
    [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")] [SerializeField] private int meleeAttackDamage = 1;
    

    [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")] [SerializeField] [Range(1, 100)] private int explosionAtttackChance = 0;
    [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")] [SerializeField] private float explosionAttackRadius = 1f;
    [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")] [SerializeField] private int explosionAttackDamage = 1;

    [FoldoutGroup("Attacks Parameters/Range Attack Parameters")] [SerializeField] [Range(1, 100)] private int rangedAtttackChance = 0;
    [FoldoutGroup("Attacks Parameters/Range Attack Parameters")] [SerializeField] private int rangeAttackDamage = 1;
    [FoldoutGroup("Attacks Parameters/Range Attack Parameters")] [SerializeField] private GameObject rangeAttackProjectile;

    private List<int> attacksChances = new List<int>();

    private void Start()
    {
        if (meleeAttack)
        {
            for (int i = 0; i < meleeAtttackChance ; i++)
            {
                attacksChances.Add(0);
            }
        }
        if (explosionAttack)
        {
            for (int i = 0; i < explosionAtttackChance ; i++)
            {
                attacksChances.Add(1);
            }
        }
        if (rangedAttack)
        {
            for (int i = 0; i < rangedAtttackChance ; i++)
            {
                attacksChances.Add(2);
            }
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
            Debug.Log("melee");
            hit.gameObject.GetComponent<PlayerLife>().Damage(meleeAttackDamage);
        }
    }

    private void ExplosionAttack()
    {
        Collider2D[] rayHits = Physics2D.OverlapCircleAll(this.transform.position, explosionAttackRadius, collisionLayerMask);
        Collider2D hit = CheckHit(rayHits);
        if (hit != null)
        {
            Debug.Log("explosion");
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

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (meleeAttack)
        {
            Gizmos.DrawWireSphere(this.transform.position+new Vector3(attackPoint.x, attackPoint.y, 0), meleeAttackRadius);
        }
        if (explosionAttack)
        {
            Gizmos.DrawWireSphere(this.transform.position, explosionAttackRadius);
        }
    }
}