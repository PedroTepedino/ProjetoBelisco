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

        public void Hit(int damage)
        {
            _lockable.Unlock();
            GetComponent<SpriteRenderer>().sprite = _activeSprite;
        }
    }
}