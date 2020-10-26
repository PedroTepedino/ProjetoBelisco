﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    [CreateAssetMenu(menuName = "Enemy Parameters")]
    public class EnemyParameters : ScriptableObject
    {
        [SerializeField] protected bool isBoss;
        [SerializeField] protected float lookingRange = 5f;
        [SerializeField] protected int maxHealthPoints = 1;
        [SerializeField] protected float alertAnimationTime = 1f;

        [BoxGroup("Movement")] [SerializeField]
        protected float movingSpeed = 5f;

        [BoxGroup("Movement")] [SerializeField]
        protected float minTimeBetweenIddles = 2f;

        [BoxGroup("Movement")] [SerializeField]
        protected float maxTimeBetweenIddles = 4f;

        [BoxGroup("Movement/Iddle")] [SerializeField]
        protected float minIddleTime;

        [BoxGroup("Movement/Iddle")] [SerializeField]
        protected float maxIddleTime = 1f;

        [BoxGroup("Movement/Grounder")] [SerializeField]
        protected Vector2 grounderCenter = Vector3.zero;

        [BoxGroup("Movement/Grounder")] [SerializeField]
        protected float grounderDistance = 1f;

        [BoxGroup("Movement/Grounder")] [SerializeField]
        protected LayerMask grounderLayerMask;

        [BoxGroup("Movement/WallDetection")] [SerializeField]
        protected Vector2 wallCheckerCenter = Vector3.zero;

        [BoxGroup("Movement/WallDetection")] [SerializeField]
        protected float wallCheckerDistance = 1f;

        [BoxGroup("Movement/WallDetection")] [SerializeField]
        protected LayerMask wallCheckerLayerMask;

        [BoxGroup("Targeting")] [SerializeField] [EnumToggleButtons]
        private LayerMask targetingLayerMask;

        [BoxGroup("Targeting")] [SerializeField]
        private Vector2 targetingCenter;

        [BoxGroup("Targeting/Boss")] [SerializeField]
        private Vector2 bossZoneCornerA;

        [BoxGroup("Targeting/Boss")] [SerializeField]
        private Vector2 bossZoneCornerB;

        public bool IsBoss => isBoss;
        public float LookingRange => lookingRange;
        public int MaxHealthPoints => maxHealthPoints;

        public float AlertAnimationTime => alertAnimationTime;

        public float MaxIddleTime => maxIddleTime;
        public float MinIddleTime => minIddleTime;

        public float MovingSpeed => movingSpeed;
        public float MinTimeBetweenIddles => minTimeBetweenIddles;
        public float MaxTimeBetweenIddles => maxTimeBetweenIddles;

        public Vector2 GrounderCenter => grounderCenter;
        public float GrounderDistance => grounderDistance;
        public LayerMask GrounderLayerMask => grounderLayerMask;

        public Vector2 WallCheckerCenter => wallCheckerCenter;
        public float WallCheckerDistance => wallCheckerDistance;
        public LayerMask WallCheckerLayerMask => wallCheckerLayerMask;

        public LayerMask TargetingLayerMask => targetingLayerMask;
        public Vector2 TargetingCenter => targetingCenter;
        public Vector2 BossZoneCornerA => bossZoneCornerA;
        public Vector2 BossZoneCornerB => bossZoneCornerB;
    }
}