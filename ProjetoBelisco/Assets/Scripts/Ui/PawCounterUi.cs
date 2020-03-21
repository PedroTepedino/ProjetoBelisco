using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PawCounterUi : MonoBehaviour
{
    private int _currentCount = 0;
    [SerializeField] private string _countFormat;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    [SerializeField] private DOTweenAnimation _shakeAnimaiton;

    private void Awake()
    {
        PlayerPawStorage.OnUpdatePawValue += UpdateValue;
        _currentCount = PlayerPawStorage.PawCount;
        UpdateValue(_currentCount);
    }

    private void OnDestroy()
    {
        PlayerPawStorage.OnUpdatePawValue -= UpdateValue;
    }

    private void OnEnable()
    {
        _currentCount = PlayerPawStorage.PawCount;
        UpdateValue(_currentCount);
    }

    private void UpdateValue(int value)
    {
        _currentCount = value;
        _text.SetText(_countFormat, _currentCount);
        _shakeAnimaiton?.DORestart();
    }
}
