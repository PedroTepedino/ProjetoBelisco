using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;

public class PlayerComboManager : MonoBehaviour
{
    [SerializeField] private int _maxcomboCount = 3;
    [ShowInInspector] [ReadOnly] public int CurrentComboCount { get; private set; } = 0;

    [SerializeField] private float _comboResetTime = 0.5f;
    [ShowInInspector] [ReadOnly] [ProgressBar(0, "_comboResetTime")] private float _comboTimer = 0f;

    [SerializeField] private List<int> _attackComboDamages;

    private PlayerAttackSystem _playerAttack;

    private void Awake()
    {
        _playerAttack = this.GetComponent<PlayerAttackSystem>();
    }

    private void OnEnable()
    {
        _comboTimer = _comboResetTime;
    }

    private void Update()
    {
        CheckResetCombo();
    }

    private void CheckResetCombo()
    {
        if (_comboTimer >= _comboResetTime)
        {
            ResetCombo();
        }
        else
        {
            _comboTimer += Time.deltaTime;
        }
    }
    
    public void Attack(Directions dir)
    {
        ResetTimer();

        if (CurrentComboCount >= 0 && CurrentComboCount < _attackComboDamages.Count)
        {
            _playerAttack.Attack(dir, _attackComboDamages[CurrentComboCount]);
        }
        else
        {
            _playerAttack.Attack(dir);   
        }
        
        CheckMaxCombo();
    }

    private void CheckMaxCombo()
    {
        if (CurrentComboCount >= _maxcomboCount)
        {
            ResetCombo();
        }
        else
        {
            CurrentComboCount++;
        }
    }

    private void ResetTimer() => _comboTimer = 0f;
    private void ResetCombo() => CurrentComboCount = 0;
}
