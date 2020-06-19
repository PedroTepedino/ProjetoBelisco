using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Enemies
{
    public abstract class IAttack : MonoBehaviour
    {
        protected Collider2D CheckHit(Collider2D[] hits)
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
}
