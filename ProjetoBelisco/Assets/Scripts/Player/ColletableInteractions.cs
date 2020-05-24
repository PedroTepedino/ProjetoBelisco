using GameScripts.Collectables;
using UnityEngine;

namespace GameScripts.Player
{
    public class ColletableInteractions : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            BaseObject collectable = collision.gameObject.GetComponent<BaseObject>();

            if (collectable != null)
            {
                collectable.Effect();
                collectable.PickUp();
            }
        }
    }
}
