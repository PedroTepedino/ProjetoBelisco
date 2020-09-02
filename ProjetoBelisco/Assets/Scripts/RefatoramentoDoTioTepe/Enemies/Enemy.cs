// using Sirenix.OdinInspector;
// using UnityEngine;

// namespace RefatoramentoDoTioTepe
// {
//     [RequireComponent(typeof(Rigidbody2D))]
//     public class Enemy : MonoBehaviour
//     {
//         [SerializeField] [AssetsOnly] [InlineEditor] private EnemyParameters _enemyParameters;
//         private IMovementController _controller;
        
//         public EnemyParameters EnemyParameters => _enemyParameters;

//         private StateMachine _stateMachine;

//         private void Awake()
//         {
//             _controller = new HorizontalMovement(this);
//         }

//         private void Update()
//         {
//             _controller.Tick();
//         }

//         private void OnDrawGizmos()
//         {
//             if (_enemyParameters == null)
//                 return;
            
//             Gizmos.color = Color.green;
//             Gizmos.DrawRay((Vector2)transform.position + _enemyParameters.EdgeCheckerCenter,
//                 Vector2.down * _enemyParameters.EdgeCheckerDistance);
//             Gizmos.DrawRay((Vector2)transform.position + _enemyParameters.WallCheckerCenter, 
//                 Vector2.right  * _enemyParameters.WallCheckerDistance);
//         }
//     }

//     public class HorizontalMovement : IMovementController
//     {
//         private readonly Rigidbody2D _rigidbody;
//         private readonly float _speed;
//         private Vector2 _edgeCheckerCenter;
//         private readonly float _edgeCheckerDistance;
//         private readonly LayerMask _edgeCheckerLayerMask;
//         private Vector2 _wallCheckerCenter;
//         private readonly float _wallCheckerDistance;
//         private readonly LayerMask _wallCheckerLayerMask;

//         private bool _lookingRight = true;
//         private bool _moving = true;

//         public HorizontalMovement(Enemy enemy)
//         {
//             _rigidbody = enemy.gameObject.GetComponent<Rigidbody2D>();
            
//             _speed = enemy.EnemyParameters.Speed;
//             _edgeCheckerCenter = enemy.EnemyParameters.EdgeCheckerCenter;
//             _edgeCheckerDistance = enemy.EnemyParameters.EdgeCheckerDistance;
//             _edgeCheckerLayerMask = enemy.EnemyParameters.EdgeCheckerLayerMask;
//             _wallCheckerCenter = enemy.EnemyParameters.WallCheckerCenter;
//             _wallCheckerDistance = enemy.EnemyParameters.WallCheckerDistance;
//             _wallCheckerLayerMask = enemy.EnemyParameters.WallCheckerLayerMask;
//         }

//         public void Tick()
//         {
//             if (_moving)
//             {
//                 _rigidbody.velocity = new Vector2(_speed * (_lookingRight ? 1f : -1f), _rigidbody.velocity.y);
//             }
//             else
//             {
//                 _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
//             }

//             if (ReachedEdge() || ReachedWall())
//             {
//                 _lookingRight = !_lookingRight;
//                 _edgeCheckerCenter.x *= -1f;
//                 _wallCheckerCenter.x *= -1f;
//             }
//         }

//         public bool ReachedEdge()
//         {
//             var raycastHit = Physics2D.Raycast((Vector2) (_rigidbody.transform.position) + _edgeCheckerCenter, Vector2.down,
//                                  _edgeCheckerDistance, _edgeCheckerLayerMask);
//             return raycastHit.collider == null;
//         }

//         public bool ReachedWall()
//         {
//             var raycastHit = Physics2D.Raycast((Vector2)(_rigidbody.transform.position) + _wallCheckerCenter, 
//                 _lookingRight ? Vector2.right : Vector2.left, _wallCheckerDistance, _wallCheckerLayerMask);
//             return raycastHit.collider != null;
//         }

//         public void ChangeMovingState()
//         {
//             _moving = !_moving;
//         }
//     }

//     public interface IMovementController
//     {
//         void Tick();
//     }
// }