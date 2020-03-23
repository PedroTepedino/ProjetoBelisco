using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackDummyLifeBar : MonoBehaviour
{
    [SerializeField] private AttackDummy _attackDummy;
    [SerializeField] private Image _lifeBar;

    private void Awake()
    {
        _attackDummy.OnLifeChange += UpdateBar;
    }

    private void OnDestroy()
    {
        _attackDummy.OnLifeChange -= UpdateBar;
    }

    private void UpdateBar(int current, int max)
    {
        _lifeBar.fillAmount = (float)(current) / (float)(max);
    }
}
