﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Player Parameters")]
    public class PlayerParameters : ScriptableObject
    {
        [BoxGroup("Movement")] [SerializeField] private float _movementSpeed = 5f;
        
        [BoxGroup("Life System")] [SerializeField] private int _maxHealth = 10;

        [BoxGroup("Grounder")] [SerializeField] private Vector3 _grounderPosition;
        [BoxGroup("Grounder")] [SerializeField] private Vector2 _grounderSizes;
        [BoxGroup("Grounder")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;


        public Vector3 GrounderPosition => _grounderPosition;
        public float MovementSpeed => _movementSpeed;
        public int MaxHealth => _maxHealth;
        public Vector2 GrounderSizes => _grounderSizes;
        public LayerMask GrounderLayerMask => _grounderLayerMask;
    }
}