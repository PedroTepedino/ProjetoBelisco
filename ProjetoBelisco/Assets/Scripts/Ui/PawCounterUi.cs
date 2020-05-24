using DG.Tweening;
using GameScripts.Player;
using UnityEngine;

namespace GameScripts.Ui
{
    public class PawCounterUi : MonoBehaviour
    {
        private int _currentCount = 0;
        [SerializeField] private string _countFormat;
        [SerializeField] private TMPro.TextMeshProUGUI _text;
        [SerializeField] private DOTweenAnimation _shakeAnimaiton;

        private void Awake()
        {
            PawStorage.OnUpdatePawValue += UpdateValue;
            _currentCount = PawStorage.PawCount;
            UpdateValue(_currentCount);
        }

        private void OnDestroy()
        {
            PawStorage.OnUpdatePawValue -= UpdateValue;
        }

        private void OnEnable()
        {
            _currentCount = PawStorage.PawCount;
            UpdateValue(_currentCount);
        }

        private void UpdateValue(int value)
        {
            _currentCount = value;
            _text.SetText(_countFormat, _currentCount);
            _shakeAnimaiton?.DORestart();
        }
    }
}
