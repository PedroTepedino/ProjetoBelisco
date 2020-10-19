using Sirenix.OdinInspector;
using UnityEditor.U2D;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    
    public class UnLockerLever : MonoBehaviour,  IHittable
    {
        [SerializeField][AssetSelector][SceneObjectsOnly] private AbstractLockable _lockable;
        //TODO : Generalizar com um animator
        [SerializeField] private Sprite _activeSprite;
        
        public void Hit(int damage)
        {
            _lockable.Unlock();
            this.GetComponent<SpriteRenderer>().sprite = _activeSprite;
        }
    }
}