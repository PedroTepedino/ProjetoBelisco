using System;
using System.Collections;
using System.Collections.Generic;
using GameScripts.PoolingSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace RefatoramentoDoTioTepe
{
    public class Attack : MonoBehaviour
    {
        [FoldoutGroup("Parameters")][SerializeField][EnumToggleButtons] private LayerMask collisionLayerMask;
        [FoldoutGroup("Parameters")][SerializeField] public float attackRange = 1f;
        [FoldoutGroup("Parameters")][SerializeField] public float attackSpeed = 1f;

        [FoldoutGroup("Attacks Parameters")][SerializeField] private bool meleeAttack = false;
        [FoldoutGroup("Attacks Parameters")][SerializeField] private bool explosionAttack = false;
        [FoldoutGroup("Attacks Parameters")][SerializeField] private bool rangedAttack = false;
        [FoldoutGroup("Attacks Parameters")][SerializeField] private bool dashAttack = false;
        [FoldoutGroup("Attacks Parameters")][SerializeField] private bool shootAndExplosionAttack = false;

        [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")][SerializeField][Range(1, 100)] private int meleeAtttackChance = 0;
        [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")][SerializeField] private Vector2 _meleeAttackCenter;
        [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")][SerializeField] private float meleeAttackRadius = 1f;
        [FoldoutGroup("Attacks Parameters/Melee Attack Parameters")][SerializeField] private int meleeAttackDamage = 1;

        [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")][SerializeField][Range(1, 100)] private int explosionAtttackChance = 0;
        [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")][SerializeField] private Vector2 _explosionAttackCenter;
        [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")][SerializeField] private float explosionAttackRadius = 1f;
        [FoldoutGroup("Attacks Parameters/Explosion Attack Parameters")][SerializeField] private int explosionAttackDamage = 1;

        [FoldoutGroup("Attacks Parameters/Range Attack Parameters")][SerializeField][Range(1, 100)] private int rangedAtttackChance = 0;
        [FoldoutGroup("Attacks Parameters/Range Attack Parameters")][SerializeField] private Vector2 _rangeAttackCenter;
        [FoldoutGroup("Attacks Parameters/Range Attack Parameters")][SerializeField] private int rangeAttackDamage = 1;
        [FoldoutGroup("Attacks Parameters/Range Attack Parameters")][SerializeField] private string _rangeAttackProjectileTag;

        [FoldoutGroup("Attacks Parameters/Dash Attack Parameters")][SerializeField][Range(1, 100)] private int dashAtttackChance = 0;
        [FoldoutGroup("Attacks Parameters/Dash Attack Parameters")][SerializeField] private Vector2 _dashAttackCenter;
        [FoldoutGroup("Attacks Parameters/Dash Attack Parameters")][SerializeField] private float dashAttackRadius = 1f;
        [FoldoutGroup("Attacks Parameters/Dash Attack Parameters")][SerializeField] private int dashAttackDamage = 1;
        [FoldoutGroup("Attacks Parameters/Dash Attack Parameters")][SerializeField] private float dashSpeed;

        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField][Range(1, 100)] private int shootAndExplosionAtttackChance = 0;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private Vector2 shootAndExplosionAttackExplosionCenter;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private float shootAndExplosionAttackExplosionRadius = 1f;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private int shootAndExplosionAttackExplosionDamage = 1;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private Vector2 shootAndExplosionAttackRangedCenter;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private int shootAndExplosionAttackRangedDamage = 1;
        [FoldoutGroup("Attacks Parameters/Shoot and Explosion Attack Parameters")][SerializeField] private string shootAndExplosionAttackProjectileTag;

        private List<int> attacksChances = new List<int>();

        public Action<int> OnAttack;

        [HideInInspector] public bool isInRange = false;

        private IEnemyStateMachine _controller;
        private GrounderEnemy grounder;
        private WallChecker wallCheck;
        private Vector2 movement = new Vector2();
        private Rigidbody2D ownerRigidbody;

        private void Awake()
        {
            _controller = this.GetComponent<IEnemyStateMachine>();
            grounder = GetComponent<GrounderEnemy>();
            wallCheck = GetComponent<WallChecker>();
            ownerRigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (meleeAttack)
            {
                for (int i = 0; i < meleeAtttackChance; i++)
                {
                    attacksChances.Add(0);
                }
            }
            if (explosionAttack)
            {
                for (int i = 0; i < explosionAtttackChance; i++)
                {
                    attacksChances.Add(1);
                }
            }
            if (rangedAttack)
            {
                for (int i = 0; i < rangedAtttackChance; i++)
                {
                    attacksChances.Add(2);
                }
            }
            if (dashAttack)
            {
                for (int i = 0; i < dashAtttackChance; i++)
                {
                    attacksChances.Add(3);
                }
            }
            if (shootAndExplosionAttack)
            {
                for (int i = 0; i < shootAndExplosionAtttackChance; i++)
                {
                    attacksChances.Add(4);
                }
            }
        }

        public void AttackAction(Transform target)
        {
            if (target != null)
            {
                int choice = Random.Range(0, attacksChances.Count);
                if (attacksChances[choice] == 0)
                {
                    OnAttack?.Invoke(0);
                    //MeleeAttack();
                }
                else if (attacksChances[choice] == 1)
                {
                    OnAttack?.Invoke(1);
                    //ExplosionAttack();
                }
                else if (attacksChances[choice] == 2)
                {
                    OnAttack?.Invoke(2);
                    //RangedAttack();
                }
                else if (attacksChances[choice] == 3)
                {
                    OnAttack?.Invoke(3);
                    //RangedAttack();
                }
                else if (attacksChances[choice] == 4)
                {
                    OnAttack?.Invoke(4);
                    //RangedAttack();
                }
            }
        }

        public void SelectedAttack(Transform target, int attackID)
        {
            if (target != null)
            {
                OnAttack?.Invoke(attackID);
            }
        }

        public void ListenAttackFinished(int index)
        {
            if (index == 0)
            {
                MeleeAttack();
            }
            else if (index == 1)
            {
                ExplosionAttack();
            }
            else if (index == 2)
            {
                RangedAttack();
            }
            else if (index == 3)
            {
                StartCoroutine(DashAttack());
            }
            else if (index == 4)
            {
                ShootAndExplosionAttack();
            }
        }

        private void Damage(int value, Collider2D hit)
        {
            var hitable = hit.gameObject.GetComponent<IHittable>();
            if (hitable != null)
            {
                hitable.Hit(value);
            }
        }

        private void MeleeAttack()
        {
            Collider2D[ ] rayHits = Physics2D.OverlapCircleAll((Vector2)(this.transform.position) + (_controller.movingRight ? _meleeAttackCenter : -_meleeAttackCenter), meleeAttackRadius, collisionLayerMask);
            Collider2D hit = CheckHit(rayHits);
            if (hit != null)
            {
                Debug.Log("melee");
                Damage(meleeAttackDamage, hit);
            }
        }

        private void ExplosionAttack()
        {
            Collider2D[ ] rayHits = Physics2D.OverlapCircleAll((Vector2)this.transform.position + (_controller.movingRight? _explosionAttackCenter: -_explosionAttackCenter), explosionAttackRadius, collisionLayerMask);
            Collider2D hit = CheckHit(rayHits);
            if (hit != null)
            {
                Debug.Log("explosion");
                Damage(explosionAttackDamage, hit);
            }
        }

        private void RangedAttack()
        {
            Pooler.Instance.SpawnFromPool(_rangeAttackProjectileTag, (Vector2)(this.transform.position) + _rangeAttackCenter, this.transform.rotation);
        }

        private IEnumerator DashAttack()
        {
            Collider2D hit = null;
            while (grounder.isGrounded && !wallCheck.wallAhead && hit == null)
            {
                Collider2D[ ] rayHits = Physics2D.OverlapCircleAll((Vector2)this.transform.position + (_controller.movingRight ? _dashAttackCenter : -_dashAttackCenter), dashAttackRadius, collisionLayerMask);
                hit = CheckHit(rayHits);
                movement.Set(_controller.movingRight ? dashSpeed : -dashSpeed, ownerRigidbody.velocity.y);
                ownerRigidbody.velocity = movement;
                if (hit != null)
                {
                    Debug.Log("dash");

                    Damage(dashAttackDamage, hit);
                }
                yield return null;
            }
        }

        private void ShootAndExplosionAttack()
        {
            Pooler.Instance.SpawnFromPool(shootAndExplosionAttackProjectileTag, (Vector2)(this.transform.position) + shootAndExplosionAttackRangedCenter, this.transform.rotation);

            Collider2D[ ] rayHits = Physics2D.OverlapCircleAll((Vector2)this.transform.position + (_controller.movingRight ? shootAndExplosionAttackExplosionCenter : -shootAndExplosionAttackExplosionCenter), shootAndExplosionAttackExplosionRadius, collisionLayerMask);
            Collider2D hit = CheckHit(rayHits);
            if (hit != null)
            {
                Debug.Log("explosion");
                Damage(shootAndExplosionAttackExplosionDamage, hit);
            }
        }

        private Collider2D CheckHit(Collider2D[ ] hits)
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
                Gizmos.DrawWireSphere(this.transform.position + new Vector3(_meleeAttackCenter.x, _meleeAttackCenter.y, 0), meleeAttackRadius);
            }
            if (explosionAttack)
            {
                Gizmos.DrawWireSphere((Vector2)this.transform.position + _explosionAttackCenter, explosionAttackRadius);
            }

            if (rangedAttack)
            {
                Gizmos.DrawLine((Vector2)this.transform.position + _rangeAttackCenter - new Vector2(0.2f, 0.2f), (Vector2)this.transform.position + _rangeAttackCenter + new Vector2(0.2f, 0.2f));
                Gizmos.DrawLine((Vector2)this.transform.position + _rangeAttackCenter - new Vector2(-0.2f, 0.2f), (Vector2)this.transform.position + _rangeAttackCenter + new Vector2(-0.2f, 0.2f));
            }

            if (dashAttack)
            {
                Gizmos.DrawWireSphere(this.transform.position + new Vector3(_dashAttackCenter.x, _dashAttackCenter.y, 0), dashAttackRadius);
            }

            if (shootAndExplosionAttack)
            {
                Gizmos.DrawWireSphere((Vector2)this.transform.position + shootAndExplosionAttackExplosionCenter, shootAndExplosionAttackExplosionRadius);

                Gizmos.DrawLine((Vector2)this.transform.position + shootAndExplosionAttackRangedCenter - new Vector2(0.2f, 0.2f), (Vector2)this.transform.position + shootAndExplosionAttackRangedCenter + new Vector2(0.2f, 0.2f));
                Gizmos.DrawLine((Vector2)this.transform.position + shootAndExplosionAttackRangedCenter - new Vector2(-0.2f, 0.2f), (Vector2)this.transform.position + shootAndExplosionAttackRangedCenter + new Vector2(-0.2f, 0.2f));
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.transform.position - new Vector3(attackRange, 0, 0), this.transform.position + new Vector3(attackRange, 0, 0));
        }
    }
}