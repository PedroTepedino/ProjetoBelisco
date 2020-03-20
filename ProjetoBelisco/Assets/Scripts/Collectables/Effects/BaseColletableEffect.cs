using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseColletableEffect : MonoBehaviour
{
    protected virtual void Awake()
    {
        
    }

    protected virtual void OnDestroy()
    {

    }

    public abstract void Effect();
}
