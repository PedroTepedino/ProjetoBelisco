using GameScripts.LivingBeingSystems;
using UnityEngine;

namespace GameScripts.Environment
{
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
            if (!collision.gameObject.activeInHierarchy) return;
        
            BaseLifeSystem lifeSystem = collision.gameObject.GetComponentInChildren<BaseLifeSystem>();

            if (lifeSystem != null)
            {
                if (lifeSystem.CurentHealth > 0)
                {
                    lifeSystem.Damage(lifeSystem.CurentHealth);
                }
            }
        }
    }
}