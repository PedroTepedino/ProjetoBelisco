using System.Collections.Generic;
using DG.Tweening;
using GameScripts.PoolingSystem;
using UnityEngine;

namespace GameScripts.Enemies
{
    public class DeadCourpse : MonoBehaviour, IPooledObject
    {
        [SerializeField] private List<SpriteRenderer> _parts;
        [SerializeField] private List<DOTweenAnimation> _animations;
    
        public void OnObjectSpawn(object[] parameters = null)
        {
            foreach (var part in _parts)
            {
                part.transform.localPosition = Vector3.zero;
            }

            foreach (var animation in _animations)
            {
                animation.DORestart();
            }
        }

        public void FadeDisable()
        {
            this.gameObject.SetActive(false);
        }
    }
}
