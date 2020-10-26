using DG.Tweening;
using UnityEngine;

namespace Belisco
{
    public class HitDummie : MonoBehaviour, IHittable
    {
        [SerializeField] private DOTweenAnimation _tween;

        private void OnValidate()
        {
            if (_tween == null) _tween = GetComponent<DOTweenAnimation>();
        }

        public void Hit(int damage)
        {
            _tween.DORestart();
        }
    }
}