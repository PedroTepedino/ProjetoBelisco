using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Belisco
{
    public class FadeInOutSceneTransition : MonoBehaviour
    {
        public static FadeInOutSceneTransition Instance;

        [SerializeField] private float _fadeInOutTime = 1.0f;
        [SerializeField] private Ease _ease;
        [SerializeField] private Volume _volume;
        [SerializeField] private Image _panel;

        private Tween FadeAnimation;
        public bool FadeInCompleted { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            FadeInCompleted = false;

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // FadeAnimation = DOTween.To(() => _volume.weight, value => _volume.weight = value, 1f, _fadeInOutTime)
            //     .SetAutoKill(false).SetEase(_ease).From(0f);
            FadeAnimation = _panel.DOFade(1f, _fadeInOutTime).SetAutoKill(false).SetEase(_ease).From(0f);
            FadeAnimation.Rewind();
            FadeAnimation.onComplete += () => FadeInCompleted = true;
        }

        private void OnValidate()
        {
            if (_volume == null) _volume = GetComponent<Volume>();
        }

        public static void LoadFadeScene()
        {
            SceneManager.LoadSceneAsync("FadeInOutScene", LoadSceneMode.Additive);
        }

        public void FadeIn()
        {
            FadeAnimation.Restart();
            FadeInCompleted = false;
        }

        public void FadeOut()
        {
            FadeAnimation.SmoothRewind();
        }
    }
}