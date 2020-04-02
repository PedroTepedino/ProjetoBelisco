using System;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseAttackSystem : SerializedMonoBehaviour
{
    [SerializeField] [FoldoutGroup("Parameters")] protected int _baseAttack = 1;
    public abstract void Attack();
}
