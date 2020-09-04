using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Enemy Parameters")]
    public class EnemyParameters : ScriptableObject
    {
        [SerializeField] protected bool isBoss;
        [SerializeField] protected float lookingRange = 5f;
        [SerializeField] protected int maxHealthPoints = 1;
        [SerializeField] protected float alertAnimationTime = 1f;

        [BoxGroup("Iddle")][SerializeField] protected float maxIddleTime = 1f;
        [BoxGroup("Iddle")][SerializeField] protected float minIddleTime = 0f;

        [BoxGroup("Movement")] [SerializeField] protected float movingSpeed = 5f;
        [BoxGroup("Movement")] [SerializeField] protected float maxStopTime = 0f;
        [BoxGroup("Movement")] [SerializeField] protected float minTimeBetweenStops = 3f;
        
        
        [BoxGroup("Movement/Grounder")] [SerializeField] protected Vector2 grounderCenter;
        [BoxGroup("Movement/Grounder")] [SerializeField] protected float grounderDistance;
        [BoxGroup("Movement/Grounder")] [SerializeField] protected LayerMask grounderLayerMask;

        [BoxGroup("Movement/WallDetection")] protected Vector3 wallCheckerTop = Vector3.zero;    
        [BoxGroup("Movement/WallDetection")] protected Vector3 wallCheckerCenter = Vector3.zero;
        [BoxGroup("Movement/WallDetection")] protected Vector3 wallCheckerBottom = Vector3.zero;
        [BoxGroup("Movement/WallDetection")] protected float wallCheckerDistance = 1f;
        [BoxGroup("Movement/WallDetection")] protected LayerMask wallCheckerLayerMask;

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