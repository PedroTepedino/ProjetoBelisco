using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Player Parameters")]
    public class PlayerParameters : ScriptableObject
    {
        [BoxGroup("Life System")] [SerializeField] private int _maxHealth = 10;
        
        [BoxGroup("Movement")] [SerializeField] private float _movementSpeed = 5f;
        [BoxGroup("Movement")] [SerializeField] private float _walkAcceleration = 5.5f;
        
        [BoxGroup("Dash")] [SerializeField]private float _dashVelocity = 10f;
        [BoxGroup("Dash")] [SerializeField]private float _dashMaxTimer = 0.5f;
        
        [BoxGroup("Jump")] [SerializeField] private float _jumpVelocity = 5f;
        [BoxGroup("Jump")] [SerializeField] private float _maxJumpButtonHoldTime = 1f;
        [BoxGroup("Jump")] [SerializeField] private float _gravityFallMultiplayer = 2f;
        [BoxGroup("Jump")] [SerializeField] private float _velocityWhenTerminateJump = 3f;
        
        [BoxGroup("Glide")] [SerializeField] private float _glideFallVelocity = 0.5f;

        [BoxGroup("Grounder")]
        [BoxGroup("Grounder/Grounder")] [SerializeField] private Vector3 _grounderPosition;
        [BoxGroup("Grounder/Grounder")] [SerializeField] private Vector2 _grounderSizes;
        [BoxGroup("Grounder/LayerMask")] [SerializeField] [EnumToggleButtons] private LayerMask _grounderLayerMask;
        [BoxGroup("Grounder/NearGroundPreview")] [SerializeField] private Vector3 _nearGroundGrounderPosition;
        [BoxGroup("Grounder/NearGroundPreview")] [SerializeField] private Vector2 _nearGroundGrounderSizes;
        
        [BoxGroup("WallJump")] [SerializeField] private Vector2 _wallJumpBoxOffsetRight;
        [BoxGroup("WallJump")] [SerializeField] private Vector2 _wallJumpBoxOffsetLeft;
        [BoxGroup("WallJump")] [SerializeField] private Vector2 _wallJumpBoxSizes;
        [BoxGroup("WallJump")] [SerializeField] private LayerMask _wallJumpLayers;
        [BoxGroup("WallJump")] [SerializeField] private float _wallJumpBoxAngle = 0;
        [BoxGroup("WallJump")] [SerializeField] private float _wallPressFallingVelocity;
        [BoxGroup("WallJump")] [SerializeField] private Vector2 _wallJumpForce;
        [BoxGroup("WallJump")] [SerializeField] private float _wallJumpMoverTimeOff = 0.1f;


        [BoxGroup("Attack")] [SerializeField] private float _timeBetweenAttacks = 0.5f;
        [BoxGroup("Attack")] [SerializeField] private LayerMask _attackLayers;
        [FoldoutGroup("Attack/BasicAttack")] [SerializeField] private int _basicAttackDamage = 1;
        
        [BoxGroup("Attack/BasicAttack/Sides")] [SerializeField] private Vector2 _basicAttackCenter;
        [BoxGroup("Attack/BasicAttack/Sides")] [SerializeField] private float _basicAttackRadius;
        [BoxGroup("Attack/BasicAttack/Up")] [SerializeField] private Vector2 _basicUpAttackCenter;
        [BoxGroup("Attack/BasicAttack/Up")] [SerializeField] private float _basicUpAttackRadius;
        [BoxGroup("Attack/BasicAttack/Down")] [SerializeField] private Vector2 _basicDownAttackCenter;
        [BoxGroup("Attack/BasicAttack/Down")] [SerializeField] private float _basicDownAttackRadius;
        
        [BoxGroup("Attack/StrongAttack")]
        [BoxGroup("Attack/StrongAttack/Ground")] [SerializeField] private Vector2 _strongAttackCenter;
        [BoxGroup("Attack/StrongAttack/Ground")] [SerializeField] private float _strongAttackRadius;
        

        public Vector3 GrounderPosition => _grounderPosition;
        public float MovementSpeed => _movementSpeed;
        public float WalkAcceleration => _walkAcceleration;
        public int MaxHealth => _maxHealth;
        public Vector2 GrounderSizes => _grounderSizes;
        public LayerMask GrounderLayerMask => _grounderLayerMask;
        public float JumpVelocity => _jumpVelocity;
        public float MaxJumpHoldTime => _maxJumpButtonHoldTime;
        public Vector2 StrongAttackCenter => _strongAttackCenter;
        public float StrongAttackRadius => _strongAttackRadius;
        public float GravityFallMultiplayer => _gravityFallMultiplayer;
        public float DashVelocity => _dashVelocity;
        public float DashMaxTimer => _dashMaxTimer;
        public float VelocityWhenTerminateJump => _velocityWhenTerminateJump;
        public Vector3 NearGroundGrounderPosition => _nearGroundGrounderPosition;
        public Vector2 NearGroundGrounderSizes => _nearGroundGrounderSizes;
        public float GlideFallVelocity => _glideFallVelocity;
        public float TimeBetweenAttacks => _timeBetweenAttacks;
        public LayerMask AttackLayers => _attackLayers;
        public int BasicAttackDamage => _basicAttackDamage;
        public Vector2 BasicAttackCenter => _basicAttackCenter;
        public float BasicAttackRadius => _basicAttackRadius;
        public Vector2 BasicUpAttackCenter => _basicUpAttackCenter;
        public float BasicUpAttackRadius => _basicUpAttackRadius;
        public Vector2 BasicDownAttackCenter => _basicDownAttackCenter;
        public float BasicDownAttackRadius => _basicDownAttackRadius;

        private Vector2 GetLeftAttackCenter(Vector2 center)
        {
            center.x *= -1;
            return center;
        }
        public AttackParameter BasicRightAttackParameter => new AttackParameter(_basicAttackCenter, _basicAttackRadius);
        public AttackParameter BasicLeftAttackParameter => new AttackParameter(GetLeftAttackCenter(_basicAttackCenter), _basicAttackRadius);
        public AttackParameter BasicUpAttackParameter => new AttackParameter(_basicUpAttackCenter, _basicUpAttackRadius);
        public AttackParameter BasicDownAttackParameter => new AttackParameter(_basicDownAttackCenter, _basicDownAttackRadius); 
        
        public AttackParameter StrongAttackGroundRight => new AttackParameter(_strongAttackCenter, _strongAttackRadius);
        public AttackParameter StrongAttackGroundLeft => new AttackParameter(GetLeftAttackCenter(_strongAttackCenter), _strongAttackRadius);

        public Vector2 WallJumpBoxOffsetRight => _wallJumpBoxOffsetRight;
        public Vector2 WallJumpBoxOffsetLeft => _wallJumpBoxOffsetLeft;
        public Vector2 WallJumpBoxSizes => _wallJumpBoxSizes;

        public LayerMask WallJumpLayers => _wallJumpLayers;

        public float WallJumpBoxAngle => _wallJumpBoxAngle;
        public float WallPressFallingVelocity => _wallPressFallingVelocity;
        public Vector2 WallJumpForce => _wallJumpForce;

        public float WallJumpMoverTimeOff
        {
            get => _wallJumpMoverTimeOff;
            set => _wallJumpMoverTimeOff = value;
        }

        
    }
}