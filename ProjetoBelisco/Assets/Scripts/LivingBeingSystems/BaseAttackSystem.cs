using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.LivingBeingSystems
{
    public abstract class BaseAttackSystem : SerializedMonoBehaviour
    {
        [SerializeField] [FoldoutGroup("Parameters")] protected int _baseAttack = 1;
        public abstract void Attack();
    }
}
