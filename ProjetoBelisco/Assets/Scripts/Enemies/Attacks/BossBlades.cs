using DG.Tweening;
using Rewired;
using UnityEngine;

namespace Belisco
{
    // [RequireComponent(typeof(Collider2D))]
    // [RequireComponent(typeof(Rigidbody2D))]
    public class BossBlades : MonoBehaviour
    {
        [SerializeField] private int baseDamage = 1;

        [HideInInspector] public int damage = 1;
        [HideInInspector] public float stabSpeed = 1;
        [HideInInspector] public float stabDistance = 1;
        [HideInInspector] public bool weaponRight;
        [HideInInspector] public Vector3 weaponInitialPosition;
        [HideInInspector] public bool isAttacking = false;

        private Rigidbody2D weaponRigidbody;
        private Vector2 movement;
        private bool going = true;
        private Transform weaponTransform;

        private void Awake()
        {
            weaponRigidbody = this.GetComponent<Rigidbody2D>();
            damage = baseDamage;
        }

        private void Update()
        {
            if (isAttacking)
            {
                Stab();
            }
        }

        private void Stab()
        {
            if ((Vector3.Distance(weaponInitialPosition, weaponTransform.position) < 0.2f) && !going)
            {
                going = true;
                isAttacking = false;
                damage = baseDamage;
                return;
            }
            movement.Set(weaponRight ? stabSpeed : -stabSpeed, weaponRigidbody.velocity.y);
            weaponRigidbody.velocity = (going ? movement : -movement);
            if (Vector3.Distance(weaponInitialPosition, weaponTransform.position) > stabDistance)
            {
                going = false;
            }
        }
        private void OnCollisionEnter2D(Collision2D hit)
        {
            Player player = hit.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Hit(damage, this.transform);
            }
        }
    }
}