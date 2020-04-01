using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;

public class PlayerComboManager : MonoBehaviour
{
    [SerializeField] private int _maxcomboCount = 3;
    [ShowInInspector] [ReadOnly] public int CurrentComboCount { get; private set; } = 0;

    [SerializeField] private float _comboResetTime = 0.5f;
    private float _comboTimer = 0f;
    
    private Queue<Directions> _attackBuffer;
    private bool _canAttackAgain = true;

    [SerializeField] private List<int> _attackComboDamages;

    private PlayerAttackSystem _playerAttack;

    public static Action OnAttackCombo;

    private void Awake()
    {
        _playerAttack = this.GetComponent<PlayerAttackSystem>();
        PlayerAnimationController.OnAttackAnimationEnd += ListenOnAttackAnimationEnd;
    }

    private void OnDestroy()
    {
        PlayerAnimationController.OnAttackAnimationEnd -= ListenOnAttackAnimationEnd;
    }

    private void OnEnable()
    {
        _comboTimer = _comboResetTime;
        _canAttackAgain = true;
        
        if (_attackBuffer == null)
        {
            _attackBuffer = new Queue<Directions>();   
        }
        else
        {
            _attackBuffer.Clear();
        }
    }

    private void Update()
    {
        CheckResetCombo();
        //AttackBufferCheck();       
    }


    private void CheckResetCombo()
    {
        if (_comboTimer >= _comboResetTime)
        {
            CurrentComboCount = 0;
            LetAttackAgain();
        }
        else
        {
            _comboTimer += Time.deltaTime;
        }
    }

    private void Attack(Directions dir)
    {
        _canAttackAgain = false;
        if (CurrentComboCount >= 0 && CurrentComboCount < _attackComboDamages.Count)
        {
            _playerAttack.Attack(dir, _attackComboDamages[CurrentComboCount]);
        }
        else
        {
            _playerAttack.Attack(dir);
        }
        StartCoroutine(UpdateCombo());
    }
    
    private IEnumerator UpdateCombo()
    {
        yield return new WaitForEndOfFrame();
        CheckMaxCombo();
    }

    public void AttackCommand(Directions dir)
    {
        ResetTimer();

        if (_canAttackAgain)
        {       
            this.Attack(dir);
        }
    }

    private void AttackBufferCheck()
    {
        if (!_canAttackAgain) return;
        
        if (_attackBuffer.Count > 0)
        {
            this.Attack(_attackBuffer.Dequeue());
        }
    }

    private void CheckMaxCombo()
    {
        if (CurrentComboCount >= _maxcomboCount)
        {
            CurrentComboCount = 0;
        }
        else
        {
            CurrentComboCount++;
        }
    }

    private void ResetTimer()
    {
        _comboTimer = 0f;
    }

    private void LetAttackAgain()
    {
        _canAttackAgain = true;
    }

    private void ListenOnAttackAnimationEnd()
    {
        LetAttackAgain();
    }
}
