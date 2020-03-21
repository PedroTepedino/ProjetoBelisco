using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCollectableObject : MonoBehaviour 
{
    public System.Action OnPickUp;

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
