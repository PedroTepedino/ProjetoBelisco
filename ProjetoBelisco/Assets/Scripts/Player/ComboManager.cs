using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameScripts.Player
{
    public class ComboManager : MonoBehaviour
    {
        [SerializeField] private int _maxcomboCount = 3;
        [ShowInInspector] [ReadOnly] public int CurrentComboCount { get; private set; } = 0;

        [SerializeField] private float _comboResetTime = 0.5f;
        private float _comboTimer = 0f;
    
        public bool CanAttackAgain { get; private set; } = true;

        [SerializeField] private List<int> _attackComboDamages;

        private AttackSystem _attack;

        public static Action OnAttackCombo;

        private void Awake()
        {
            _attack = this.GetComponent<AttackSystem>();
        }

        private void OnEnable()
        {
            _comboTimer = _comboResetTime;
            CanAttackAgain = true;
        }

        private void Update()
        {
            CheckResetCombo();       
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
            CanAttackAgain = false;

            if (CurrentComboCount >= _maxcomboCount)
            {
                CheckMaxCombo();
            }
        
            if (CurrentComboCount >= 0 && CurrentComboCount < _attackComboDamages.Count)
            {
                _attack.Attack(dir, _attackComboDamages[CurrentComboCount]);
            }
            else
            {
                _attack.Attack(dir);
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

            if (CanAttackAgain)
            {       
                this.Attack(dir);
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
            CanAttackAgain = true;
        }

        private void ListenOnAttackAnimationEnd()
        {
            LetAttackAgain();
        }
    }
}
