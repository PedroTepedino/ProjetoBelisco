using DG.Tweening;
using RefatoramentoDoTioTepe;
using GameScripts.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameScripts.Ui
{
    public class RespawnerUi : MonoBehaviour
    {
        private Tween _animation = null;
        [SerializeField] private Volume _postProcessingVolume;

        [SerializeField] private GameObject _menu;
        [SerializeField] private TMPro.TextMeshProUGUI _timerText;

        private PlayerRespawner _curentPlayerRespawner = null;

        private void Awake()
        {
            Life.OnPlayerSpawn += FadeOutScene;
            PlayerRespawner.OnStartTimer += ListenOnStartTimer;
        }

        private void OnDestroy()
        {
            Life.OnPlayerSpawn -= FadeOutScene;
            PlayerRespawner.OnStartTimer -= ListenOnStartTimer;
        }

        private void OnEnable()
        {
            KillAnimation();

            _animation = DOTween.To(() => _postProcessingVolume.weight, x => _postProcessingVolume.weight = x, 1, 1f).From(0f).SetAutoKill(true);
            _menu.SetActive(true);
            _timerText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_curentPlayerRespawner != null)
            {
                _timerText.text = "Respawn in: " + ((int)Mathf.CeilToInt(_curentPlayerRespawner.RemainigTime)).ToString();
            }
        }

        public void Continue()
        {
            _menu.SetActive(false);
            PlayerRespawner.CurrentSpawner?.StartPlayerRespawnProcess();
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void FadeOutScene(GameObject obj)
        {
            KillAnimation();

            _animation = DOTween.To(() => _postProcessingVolume.weight, x => _postProcessingVolume.weight = x, 0, 0.2f);
            _animation.OnComplete(new TweenCallback(() => UnloadScene()));
        }

        private void UnloadScene()
        {
            _curentPlayerRespawner = null;
        }

        private void KillAnimation()
        {
            _animation?.Kill();
        }

        private void ListenOnStartTimer(PlayerRespawner respawner)
        {
            _curentPlayerRespawner = respawner;
            _timerText.gameObject.SetActive(true);
        }
    }
}
