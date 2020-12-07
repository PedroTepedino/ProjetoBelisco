using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class MovingPlatformController : AbstractLockable
    {
        [SerializeField] private Transform _actualPlatform;
        [SerializeField] private Transform _finalDestination;

        [BoxGroup("Parameters")] [SerializeField]
        private float _velocity = 5f;

        [BoxGroup("Parameters")] [SerializeField]
        private Ease _easeType = Ease.Linear;

        private Tween _moveAnimation;

        private void Awake()
        {
            _moveAnimation = _actualPlatform.DOMove(_finalDestination.position, _velocity).From(transform.position)
                .SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetEase(_easeType);
            _moveAnimation.Rewind();
        }

        private void OnValidate()
        {
            if (_actualPlatform == null)
            {
                MovingPlatform aux = GetComponentInChildren<MovingPlatform>();
                if (aux != null)
                {
                    _actualPlatform = aux.transform;
                }
                else
                {
                    GameObject obj = new GameObject("ActualMovingPlatform");
                    obj.AddComponent<BoxCollider2D>();
                    obj.AddComponent<MovingPlatform>();
                    _actualPlatform = obj.transform;
                    _actualPlatform.parent = transform;
                    _actualPlatform.localPosition = Vector3.zero;
                }
            }

            if (_finalDestination == null)
            {
                if (transform.childCount > 1)
                {
                    _finalDestination = transform.GetChild(transform.childCount - 1);
                }
                else
                {
                    _finalDestination = new GameObject("FinalPoint").transform;
                    _finalDestination.parent = transform;
                    _finalDestination.localPosition = Vector3.zero;
                }
            }
        }

        public override void Unlock()
        {
            _moveAnimation.Play();
        }

        public override void Lock()
        {
            _moveAnimation.Pause();
        }
    }
}