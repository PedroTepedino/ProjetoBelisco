using System;
using DG.Tweening;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class HitDummie : MonoBehaviour, IHittable
    {
        [SerializeField] private DOTweenAnimation _tween;
        
        public void Hit(int damage)
        {
            _tween.DORestart();
        }

        private void OnValidate()
        {
            if (_tween == null)
            {
                _tween = this.GetComponent<DOTweenAnimation>();
            }
        }
    }
}
