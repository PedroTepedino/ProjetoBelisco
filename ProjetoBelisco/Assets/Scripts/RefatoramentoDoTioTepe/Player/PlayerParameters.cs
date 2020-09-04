using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Player Parameters")]
    public class PlayerParameters : ScriptableObject
    {
        [BoxGroup("Movement")] [SerializeField] private float _movementSpeed = 5f;
        
        [BoxGroup("Dash")] [SerializeField]private float _dashVelocity = 10f;
        [BoxGroup("Dash")] [SerializeField]private float _dashMaxTimer = 0.5f;
        
        [BoxGroup("Jump")] [SerializeField] private float _jumpVelocity = 5f;
        [BoxGroup("Jump")] [SerializeField] private float _maxJumpButtonHoldTime = 1f;
        [BoxGroup("Jump")] [SerializeField] private float _gravityFallMultiplayer = 2f;
        [BoxGroup("Jump")] [SerializeField] private float _velocityWhenTerminateJump = 3f;

        [BoxGroup("Life System")] [SerializeField] private int _maxHealth = 10;

        [BoxGroup("Grounder")] [SerializeField] private Vector3 _grounderPosition;
        [BoxGroup("Grounder")] [SerializeField] private Vector2 _grounderSizes;
        [BoxGroup("Grounder")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;

        [BoxGroup("Attack")]
        [BoxGroup("Attack/StrongAttack")] [SerializeField] private Vector2 _strongAttackCenter;
        [BoxGroup("Attack/StrongAttack")] [SerializeField] private float _strongAttackRadius;
        [BoxGroup("Attack/StrongAttack")] [SerializeField] private Vector2 _strongAttackExplosionCenter;
        [BoxGroup("Attack/StrongAttack")] [SerializeField] private float _strongAttackExplosionRadius;


        public Vector3 GrounderPosition => _grounderPosition;
        public float MovementSpeed => _movementSpeed;
        public int MaxHealth => _maxHealth;
        public Vector2 GrounderSizes => _grounderSizes;
        public LayerMask GrounderLayerMask => _grounderLayerMask;
        public float JumpVelocity => _jumpVelocity;
        public float MaxJumpHoldTime => _maxJumpButtonHoldTime;
        public Vector2 StrongAttackCenter => _strongAttackCenter;
        public float StrongAttackRadius => _strongAttackRadius;
        public Vector2 StrongAttackExplosionCenter => _strongAttackExplosionCenter;
        public float StrongAttackExplosionRadius => _strongAttackExplosionRadius;
        public float GravityFallMultiplayer => _gravityFallMultiplayer;
        public float DashVelocity => _dashVelocity;
        public float DashMaxTimer => _dashMaxTimer;
        public float VelocityWhenTerminateJump => _velocityWhenTerminateJump;
    }
}