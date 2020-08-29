using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Enemy Parameters")]
    public class EnemyParameters : ScriptableObject
    {
        [BoxGroup("Movement")] [SerializeField] private float _speed = 5f;
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private Vector2 _edgeCheckerCenter;
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private float _edgeCheckerDistance;
        [BoxGroup("Movement/EdgeDetection")] [SerializeField] private LayerMask _edgeCheckerLayerMask;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private Vector2 _wallCheckerCenter;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private float _wallCheckerDistance;
        [BoxGroup("Movement/WallDetection")] [SerializeField] private LayerMask _wallCheckerLayerMask;

        public float Speed => _speed;

        public Vector2 EdgeCheckerCenter => _edgeCheckerCenter;
        public float EdgeCheckerDistance => _edgeCheckerDistance;
        public LayerMask EdgeCheckerLayerMask => _edgeCheckerLayerMask;

        public Vector2 WallCheckerCenter => _wallCheckerCenter;
        public float WallCheckerDistance => _wallCheckerDistance;
        public LayerMask WallCheckerLayerMask => _wallCheckerLayerMask;
    }
}