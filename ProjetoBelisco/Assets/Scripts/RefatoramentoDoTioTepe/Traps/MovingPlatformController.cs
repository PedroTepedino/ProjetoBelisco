using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class MovingPlatformController : MonoBehaviour
    {
        [SerializeField] private Transform _actualPlatform;
        [SerializeField] private Transform _finalDestination;

        private Tween _moveAnimation;

        [BoxGroup("Parameters")] [SerializeField]
        private float _velocity = 5f;

        [BoxGroup("Parameters")] [SerializeField]
        private Ease _easeType = Ease.Linear;

        private void Awake()
        {
            _moveAnimation = _actualPlatform.DOMove(_finalDestination.position, _velocity).From(this.transform.position)
                .SetSpeedBased(true).SetLoops(-1, LoopType.Yoyo).SetEase(_easeType);
            _moveAnimation.Rewind();
        }

        private void OnEnable()
        {
            UnlockPlatform();   
        }

        public void UnlockPlatform()
        {
            _moveAnimation.Play();
        }

        public void LockPlatform()
        {
            _moveAnimation.Pause();
        }

        private void OnValidate()
        {
            if (_actualPlatform == null)
            {
                var aux = this.GetComponentInChildren<MovingPlatform>();
                if (aux != null)
                    _actualPlatform = aux.transform;
                else
                {
                    var obj = new GameObject("ActualMovingPlatform");
                    obj.AddComponent<BoxCollider2D>();
                    obj.AddComponent<MovingPlatform>();
                    _actualPlatform = obj.transform;
                    _actualPlatform.parent = this.transform;
                    _actualPlatform.localPosition = Vector3.zero;
                }
            }
            
            if (_finalDestination == null)
            {
                if (this.transform.childCount > 1)
                {
                    _finalDestination = this.transform.GetChild(this.transform.childCount - 1);
                }
                else
                {
                    _finalDestination = new GameObject("FinalPoint").transform;
                    _finalDestination.parent = this.transform;
                    _finalDestination.localPosition = Vector3.zero;
                }
            }
        }
    }
}