using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Belisco
{
    public class LifeBarController : MonoBehaviour
    {
        [SerializeField] [BoxGroup("Components")] [Required]
        private Image _barFillImage;

        [SerializeField] [BoxGroup("Components")] [Required]
        private Image _barFillDecay;

        [SerializeField] [BoxGroup("Components")]
        private DOTweenAnimation _shakeAnimation;

        [SerializeField] [BoxGroup("Decay")] private float _decaySpeed = 0.5f;
        [SerializeField] [BoxGroup("Decay")] private float _decayDelay = 0.75f;

        [SerializeField] [BoxGroup("Heal Decay")]
        private float _healDecaySpeed = 1f;

        [SerializeField] [BoxGroup("Heal Decay")]
        private float _healDecayDelay = 0.2f;


        private float _curentFill = 1f;
        private Tween _decayBarAnimation;

        private void Awake()
        {
            Player.OnPlayerDamage += ListenDamage;
            Player.OnPlayerHeal += ListenHeal;
        }

        private void OnDestroy()
        {
            Player.OnPlayerDamage -= ListenDamage;
            Player.OnPlayerHeal -= ListenHeal;
        }

        private void ListenDamage(int curentHealth, int maxHealth)
        {
            _curentFill = curentHealth / (float) maxHealth;
            _barFillImage.fillAmount = _curentFill;

            ShakeBar();
            LifeBarDecay();
        }

        private void ListenHeal(int curentHealth, int maxHealth)
        {
            _curentFill = curentHealth / (float) maxHealth;
            _barFillDecay.fillAmount = _curentFill;

            LifeBarHealDecay();
        }

        private void ShakeBar()
        {
            _shakeAnimation?.DORestart();
        }

        private void LifeBarDecay()
        {
            if (_decayBarAnimation != null)
            {
                _decayBarAnimation.Kill();
            }

            _decayBarAnimation = DOTween.To(() => _barFillDecay.fillAmount, x => _barFillDecay.fillAmount = x,
                    _curentFill, _decaySpeed)
                .SetDelay(_decayDelay).SetAutoKill(false).SetSpeedBased(true).SetEase(Ease.Linear);
        }

        private void LifeBarHealDecay()
        {
            if (_decayBarAnimation != null)
            {
                _decayBarAnimation.Kill();
            }

            _decayBarAnimation = DOTween.To(() => _barFillImage.fillAmount, x => _barFillImage.fillAmount = x,
                    _curentFill, _healDecaySpeed)
                .SetDelay(_healDecayDelay).SetAutoKill(false).SetSpeedBased(true).SetEase(Ease.Linear);
        }
    }
}