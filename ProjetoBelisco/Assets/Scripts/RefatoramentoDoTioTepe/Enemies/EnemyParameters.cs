using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Enemy Parameters")]
    public class EnemyParameters : ScriptableObject
    {
        [SerializeField] private bool isBoss;
        [SerializeField] private float lookingRange = 5f;
        
        private float alertAnimationTime = 1f;

        [BoxGroup("Iddle")][SerializeField] private float maxIddleTime = 1f;
        [BoxGroup("Iddle")][SerializeField] private float minIddleTime = 0f;

        [BoxGroup("Movement")] [SerializeField] private float movingSpeed = 5f;
        [BoxGroup("Movement")] [SerializeField] private float maxStopTime = 0f;
        [BoxGroup("Movement")] [SerializeField] private float minTimeBetweenStops = 3f;
        
        
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private Vector2 _edgeCheckerCenter;
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private float _edgeCheckerDistance;
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private LayerMask _edgeCheckerLayerMask;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private Vector2 _wallCheckerCenter;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private float _wallCheckerDistance;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private LayerMask _wallCheckerLayerMask;

        public bool IsBoss => isBoss;
        public float LookingRange => lookingRange;

        public float AlertAnimationTime => alertAnimationTime;
        
        public float MaxIddleTime => maxIddleTime;
        public float MinIddleTime => minIddleTime;

        public float MovingSpeed => movingSpeed;
        public float MaxStopTime => maxStopTime;
        public float MinTimeBetweenStops => minTimeBetweenStops;

        public Vector2 EdgeCheckerCenter => _edgeCheckerCenter;
        public float EdgeCheckerDistance => _edgeCheckerDistance;
        public LayerMask EdgeCheckerLayerMask => _edgeCheckerLayerMask;

        public Vector2 WallCheckerCenter => _wallCheckerCenter;
        public float WallCheckerDistance => _wallCheckerDistance;
        public LayerMask WallCheckerLayerMask => _wallCheckerLayerMask;
    }
}