using UnityEngine;
using Sirenix.OdinInspector;

public abstract class BaseCollectableObject : MonoBehaviour
{
    [SerializeField] [EnumToggleButtons] private CollectableTypes _type;

    public System.Action OnPickUp;

    public CollectableTypes Type { get => _type; set => _type = value; }

    public abstract void PickUp();

    public abstract void Effect();

    private void OnDisable()
    {
        if (OnPickUp != null)
        {
            System.Delegate[] aux = OnPickUp.GetInvocationList();
            foreach(System.Delegate del in aux)
            {
                OnPickUp -= del as System.Action;
            }
        }
    }
}
