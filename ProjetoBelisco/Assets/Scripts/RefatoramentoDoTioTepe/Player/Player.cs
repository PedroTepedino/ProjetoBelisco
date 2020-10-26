using System;
using System.Collections;
using Cinemachine;
using GameScripts.PoolingSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public enum Directions
    {
        Right = 1,
        Up,
        Left,
        Down,
    }

    [RequireComponent(typeof(PlayerAnimatorController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IHittable , IPooledObject
    {
        [SerializeField] private PlayParticlesGeneral _hurtParticles;
        [SerializeField] [AssetsOnly] [FoldoutGroup("Player Parameters")] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;

        private IJumper _jumper;
        private IAttacker _attacker;
        //private List<IAttacker> _attackerList;
        private LifeSystem _lifeSystem;
        private Grounder _grounder;
        private PlayerLocker _playerLocker;
        private Glider _glider;
        private WallJumper _wallJumper;

        [SerializeField] 
        private PlayerAnimatorController _playerAnimatorController;

        private bool _lastFrameJumpState = false;
        private bool _justTouchedGround = false;
        private bool _attacking = false;
        private bool _canDash = true;

        public bool CanMove = true;
        public bool IsDashing => _mover is Dasher;

        public event Action OnJump;
        public event Action OnTouchedGround;

        public PlayerParameters PlayerParameters => _playerParameters;
        public LifeSystem LifeSystem => _lifeSystem;
        public Grounder Grounder => _grounder;
        public bool Jumping => _jumper.Jumping;
        public bool Gliding => _glider.Gliding;
        public PlayerAnimatorController AnimatorController => _playerAnimatorController;
        
        public static event Action<GameObject> OnPlayerSpawn;
        public static event Action OnPlayerDeath;
        public static event Action<int, int> OnPlayerDamage;
        
        public static event Action<int, int> OnPlayerHeal;

        [SerializeField] private PlayerGhostSpawner _ghostSpawner;
        private WaitForSeconds _waitForSecondsDash;
        
        private void Awake()
        {
            AttackerTimer.SetTimer(_playerParameters.TimeBetweenAttacks);
            _waitForSecondsDash = new WaitForSeconds(_playerParameters.TimeBetweenDashes);
            
            _grounder = new Grounder(this);
            _lifeSystem = new LifeSystem(this);
            _mover = new NewMover(this);
            _jumper = new Jumper(this);
            _playerLocker = new PlayerLocker(this);
            _glider = new Glider(this);
            _attacker = new BasicAttacker(this);
            _wallJumper = new WallJumper(this);

            // _attackerList = new List<IAttacker>();
            // _attackerList.Add(new BasicAttacker(this));
            // _attackerList.Add(new StrongAttacker(this));
        }

        private void OnEnable()
        {
            _grounder = new Grounder(this);
            _mover = new NewMover(this);
            _jumper = new NewJumper(this);
            _playerLocker = new PlayerLocker(this);
            _glider = new Glider(this);
            _attacker = new BasicAttacker(this);
            
            _grounder.Tick();
            
            var camera = UnityEngine.Object.FindObjectOfType<CinemachineVirtualCamera>();
            
            if (camera == null ) return;

            camera.Follow = this.transform;
        }

        public void Hit(int damage)
        {
            _hurtParticles.EmitParticle();
            _playerAnimatorController.Hurt();
            _lifeSystem.Damage(damage);
        }

        private void Update()
        {
            AttackerTimer.SubtractTimer();
            
            _grounder.Tick();
            
            if (!_attacking && CanMove)
            { 
                _mover.Tick();

                if (!(_mover is Dasher))
                {
                    _jumper.Tick();
                    _wallJumper.Tick();
                }
                
                if (Dasher.CheckDashInput())
                {
                    StartDash();
                }
            }
            
            _attacker.Tick();
            //_attackerList.ForEach(attacker => attacker.Tick());
            
            CheckJumping();

            var isGrounded = _grounder.IsGrounded;
            AnimatorController.UpdateParameters(isGrounded);

            JustTouchedGround(isGrounded);
        }

        private void StartDash()
        {
            if (_canDash)
            {
                _mover = new Dasher(this);
                _playerAnimatorController.Dash();
                _ghostSpawner.StartSpawning();
                _canDash = false;
            }
        }

        public void StopDashing()
        {
            if (_mover is Dasher dasher)
            {
                dasher?.StopDashing();
            }
        }

        public void EndDash()
        {
            _mover = new NewMover(this);
            StartCoroutine(DashTimer());
        }

        private IEnumerator DashTimer()
        {
            yield return _waitForSecondsDash;
            _canDash = true;
        }

        //public void CallAttack<T>() where T : IAttacker
        public void CallAttack()
        {
            _attacker.Attack();
            //_attackerList.Find(item => item is T)?.Attack();
        }

        public void StartAttack()
        {
            _attacking = true;
            _playerLocker.LockPlayer();
        }

        public void EndAttack()
        {
            _attacking = false;
            _playerLocker.UnlockPlayer();
            AttackerTimer.ResetTimer();
        }

        public Directions GetAttackDirection()
        {
            float vertical = RewiredPlayerInput.Instance.Vertical;
            Directions direction;

            if (vertical > 0.25f)
            {
                direction = Directions.Up;
            }
            else if (vertical < -0.25f && !_grounder.IsGrounded)
            {
                direction = Directions.Down;
            }
            else
            {
                if (_playerAnimatorController.IsLookingRight)
                {
                    direction = Directions.Right;
                }
                else
                {
                    direction = Directions.Left;
                }
            }

            return direction;
        }

        public Directions GetDirectionHorizontal() => _playerAnimatorController.IsLookingRight ? Directions.Right : Directions.Left;

        private void CheckJumping()
        {
            if (!_lastFrameJumpState && _jumper.Jumping)
            {
                OnJump?.Invoke();
            }

            _lastFrameJumpState = _jumper.Jumping;
        }

        private void JustTouchedGround(bool isGrounded)
        {
            if (!_justTouchedGround && isGrounded)
            {
                OnTouchedGround?.Invoke();
            }

            _justTouchedGround = isGrounded;
        }

        private void OnValidate()
        {
            if (_playerParameters == null)
                _playerParameters = Resources.Load<PlayerParameters>("ScriptableObjects/DefaultPlayerParameters");

            if (AnimatorController == null)
                _playerAnimatorController = this.GetComponent<PlayerAnimatorController>();

            if (_ghostSpawner == null)
                _ghostSpawner = this.GetComponent<PlayerGhostSpawner>();
        }

        public void OnObjectSpawn(object[] parameters = null)
        {
            if (!_lifeSystem.StillAlive)
                this._lifeSystem = new LifeSystem(this);
            
            OnPlayerHeal?.Invoke(_lifeSystem.CurrentLife, _lifeSystem.MaxHealth);
            
            OnPlayerSpawn?.Invoke(this.gameObject); 
            this.gameObject.SetActive(true);
        }

        public void Died()
        {
            OnPlayerDeath?.Invoke();
        }

        public void DamageDealt(int currentLife, int maxLife)
        {
            OnPlayerDamage?.Invoke(currentLife, maxLife);
        }

        [Button]
        public void Damage()
        {
            this.Hit(1);
        }
        
#if UNITY_EDITOR
        [SerializeField] [EnumToggleButtons] private GizmosType _gizmosToShow = 0;
        private void OnDrawGizmos()
        {
            var position = this.transform.position;
            
            foreach (GizmosType gizmo in Enum.GetValues(typeof(GizmosType)))
            {
                if ((_gizmosToShow & gizmo) == gizmo)
                {
                    switch (gizmo)
                    {
                        case GizmosType.Grounder:
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(position + _playerParameters.GrounderPosition, _playerParameters.GrounderSizes);
                            break;  
                        case GizmosType.NearGround:
                            Gizmos.color = new Color(0f, 0.5f, 0f, 1f);
                            Gizmos.DrawWireCube(position + _playerParameters.NearGroundGrounderPosition,
                                _playerParameters.NearGroundGrounderSizes);
                            break;
                        case GizmosType.BasicAttack:
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireSphere(position + (Vector3) _playerParameters.BasicAttackCenter,
                                _playerParameters.BasicAttackRadius);
                            var leftAttackCenter = (Vector3) _playerParameters.BasicAttackCenter;
                            leftAttackCenter.x *= -1;
                            Gizmos.DrawWireSphere(position + leftAttackCenter, _playerParameters.BasicAttackRadius);
                            break;
                        case GizmosType.BasicAttackUp:
                            Gizmos.color = Color.magenta;
                            Gizmos.DrawWireSphere(position + (Vector3) _playerParameters.BasicUpAttackCenter,
                                _playerParameters.BasicUpAttackRadius);
                            break;
                        case GizmosType.BasicAttackDown:
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawWireSphere(position + (Vector3) _playerParameters.BasicDownAttackCenter,
                                _playerParameters.BasicDownAttackRadius);
                            break;
                        case GizmosType.WallChecker:
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(position + (Vector3)_playerParameters.WallJumpBoxOffsetRight, (Vector3)_playerParameters.WallJumpBoxSizes);
                            Gizmos.DrawWireCube(position + (Vector3)_playerParameters.WallJumpBoxOffsetLeft, (Vector3)_playerParameters.WallJumpBoxSizes);
                            
                            break;
                    }  
                }
            }
        }
        [Flags] public enum GizmosType
        {
            Grounder = 1 << 0,
            NearGround = 1 << 1,
            BasicAttack = 1 << 2,
            BasicAttackUp = 1 << 3,
            BasicAttackDown = 1 << 4,
            WallChecker = 1 << 5,
        }
#endif //UNITY_EDITOR
        
    }
}
