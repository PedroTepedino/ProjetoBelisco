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

        // TODO: add knockback if necessary
        public void Hit(int damage, Transform attacker)
        {
            _tween.DORestart();
        }
    }
}