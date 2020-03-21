using UnityEngine;

public class PlayerColletableInteractions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseCollectableObject collectable = collision.gameObject.GetComponent<BaseCollectableObject>();

        if (collectable != null)
        {
            collectable.Effect();
            collectable.PickUp();
        }
    }
}
