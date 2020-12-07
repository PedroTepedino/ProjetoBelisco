using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Belisco
{

    public class Attack : MonoBehaviour
    {
        [FoldoutGroup("Parameters")][SerializeField][EnumToggleButtons]
        private LayerMask collisionLayerMask;

        [FoldoutGroup("Parameters")][SerializeField]
        public float attackRange = 1f;

        [FoldoutGroup("Parameters")][SerializeField]
        public float attackSpeed = 1f;

        [FoldoutGroup("Parameters")][SerializeField][AssetsOnly][InlineEditor]
        private BaseAttack[ ] avalibleAttacks;

        private EnemyAnimationController animationController;
        private EnemyStateMachine _controller;
        private int attackChancesSum = 1;
        private int selectedAttackIndex;

        [HideInInspector] public bool isInRange;

        private void Awake()
        {
            _controller = GetComponent<EnemyStateMachine>();
            animationController = GetComponent<EnemyAnimationController>();
        }

        private void Start()
        {
            for (int i = 0; i < avalibleAttacks.Length; i++)
            {
                attackChancesSum += avalibleAttacks[i].attackChance;
            }
        }

        public void SelectAttack()
        {
            int assortedChance = Random.Range(0, (attackChancesSum));
            int accumulatedChance = 0;

            for (int i = 0; i < avalibleAttacks.Length; i++)
            {
                accumulatedChance += avalibleAttacks[i].attackChance;
                if (assortedChance <= accumulatedChance)
                {
                    animationController.TriggerAnimationAttack(avalibleAttacks[i].attackAnimationName);
                    selectedAttackIndex = i;
                    break;
                }
            }
        }

        public void ListenAttackFinished()
        {
            avalibleAttacks[selectedAttackIndex].AttackAction(this.transform, collisionLayerMask, _controller);
        }
    }
}