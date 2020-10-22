using System.Linq;
using GameScripts.PoolingSystem;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Ghost : MonoBehaviour, IPooledObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public void OnObjectSpawn(object[] parameters = null)
        {
            if (parameters != null)
            {
                SetSprite(parameters.ToList().Find(obj => obj is Sprite) as Sprite);
            }
        }

        private void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = this.GetComponent<SpriteRenderer>();
            }
        }
    }
}