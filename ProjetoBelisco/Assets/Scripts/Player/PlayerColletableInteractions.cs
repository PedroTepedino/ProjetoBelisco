using UnityEngine;

public class PlayerColletableInteractions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICollectableObject collectable = collision.gameObject.GetComponent<ICollectableObject>();

        if (collectable != null)
        {
            collectable.Effect();
            collectable.PickUp();
        }
    }
}
