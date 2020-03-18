using UnityEngine;

public abstract class BaseAttackSystem : MonoBehaviour
{
    [SerializeField] [Sirenix.OdinInspector.FoldoutGroup("Parameters")] protected int _baseAttack = 1;
    public abstract void Attack();
}
