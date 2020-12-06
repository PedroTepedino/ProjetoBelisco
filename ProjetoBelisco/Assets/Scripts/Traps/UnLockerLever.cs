using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class UnLockerLever : MonoBehaviour, IHittable
    {
        [SerializeField] [AssetSelector] [SceneObjectsOnly]
        private AbstractLockable _lockable;

        //TODO : Generalizar com um animator
        [SerializeField] private Sprite _activeSprite;

        // TODO: add knockback if necessary
        public void Hit(int damage, Transform attacker)
        {
            _lockable.Unlock();
            GetComponent<SpriteRenderer>().sprite = _activeSprite;
        }
    }
}