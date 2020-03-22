using UnityEngine;

public class PawCollectable : BaseCollectableObject, IPooledObject
{
    [SerializeField] private int _pawsToAdd = 1;

    public override void PickUp()
    {
        OnPickUp?.Invoke();
        this.gameObject.SetActive(false);
    }

    public override void Effect()
    {
        PlayerPawStorage.AddPaws(_pawsToAdd);
    }

    public void OnObjectSpawn()
    {
        this.gameObject.SetActive(true);
    }
}
