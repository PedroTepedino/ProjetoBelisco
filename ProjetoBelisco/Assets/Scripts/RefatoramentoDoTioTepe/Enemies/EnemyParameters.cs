using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Enemy Parameters")]
    public class EnemyParameters : ScriptableObject
    {
        [SerializeField] private bool isBoss;
        [SerializeField] private float lookingRange = 5f;
        [SerializeField] private int maxHealthPoints = 1;
        
        
        private float alertAnimationTime = 1f;

        [BoxGroup("Iddle")][SerializeField] private float maxIddleTime = 1f;
        [BoxGroup("Iddle")][SerializeField] private float minIddleTime = 0f;

        [BoxGroup("Movement")] [SerializeField] private float movingSpeed = 5f;
        [BoxGroup("Movement")] [SerializeField] private float maxStopTime = 0f;
        [BoxGroup("Movement")] [SerializeField] private float minTimeBetweenStops = 3f;
        
        
        [BoxGroup("Movement/Grounder")] [SerializeField] private Vector2 grounderCenter;
        [BoxGroup("Movement/Grounder")] [SerializeField] private float grounderDistance;
        [BoxGroup("Movement/Grounder")] [SerializeField] private LayerMask grounderLayerMask;

        [BoxGroup("Movement/WallDetection")] private Vector3 wallCheckerTop = Vector3.zero;    
        [BoxGroup("Movement/WallDetection")] private Vector3 wallCheckerCenter = Vector3.zero;
        [BoxGroup("Movement/WallDetection")] private Vector3 wallCheckerBottom = Vector3.zero;
        [BoxGroup("Movement/WallDetection")] private float wallCheckerDistance = 1f;
        [BoxGroup("Movement/WallDetection")] private LayerMask wallCheckerLayerMask;

        public bool IsBoss => isBoss;
        public float LookingRange => lookingRange;
        public int MaxHealthPoints => maxHealthPoints;

        public float AlertAnimationTime => alertAnimationTime;
        
        public float MaxIddleTime => maxIddleTime;
        public float MinIddleTime => minIddleTime;

        public float MovingSpeed => movingSpeed;
        public float MaxStopTime => maxStopTime;
        public float MinTimeBetweenStops => minTimeBetweenStops;

        public Vector2 GrounderCenter => grounderCenter;
        public float GrounderDistance => grounderDistance;
        public LayerMask GrounderLayerMask => grounderLayerMask;

        public Vector2 WallCheckerTop => wallCheckerTop;
        public Vector2 WallCheckerCenter => wallCheckerCenter;
        public Vector2 WallCheckerBottom => wallCheckerBottom;
        public float WallCheckerDistance => wallCheckerDistance;
        public LayerMask WallCheckerLayerMask => wallCheckerLayerMask;
    }
}