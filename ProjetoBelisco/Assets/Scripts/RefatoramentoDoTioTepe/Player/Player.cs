using System;
using DG.Tweening;
using GameScripts.PoolingSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IHittable , IPooledObject
    {
        [SerializeField] private PlayParticlesGeneral _hurtParticles;
        [SerializeField] [AssetsOnly] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;
        private IJumper _jumper;
        private IAttacker _attacker;
        //private List<IAttacker> _attackerList;
        private LifeSystem _lifeSystem;
        private Grounder _grounder;
        private PlayerLocker _playerLocker;
        private Glider _glider;

        [SerializeField] 
        private PlayerAnimatorController _playerAnimatorController;

        private bool _lastFrameJumpState = false;
        private bool _justTouchedGround = false;
        private bool _attacking = false;

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


        private void Awake()
        {
            AttackerTimer.SetTimer(_playerParameters.TimeBetweenAttacks);
            
            _grounder = new Grounder(this);
            _lifeSystem = new LifeSystem(this);
            _mover = new Mover(this);
            _jumper = new Jumper(this);
            _playerLocker = new PlayerLocker(this);
            _glider = new Glider(this);
            _attacker = new BasicAttacker(this);

            // _attackerList = new List<IAttacker>();
            // _attackerList.Add(new BasicAttacker(this));
            // _attackerList.Add(new StrongAttacker(this));
        }

        private void OnEnable()
        {
            _grounder = new Grounder(this);
            _mover = new Mover(this);
            _jumper = new Jumper(this);
            _playerLocker = new PlayerLocker(this);
            _glider = new Glider(this);
            _attacker = new BasicAttacker(this);
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
            
            if (!_attacking)
            {
                _mover.Tick();

                if (!(_mover is Dasher))
                {
                    _glider.Tick();
                    
                    _jumper.Tick();
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
            _mover = new Dasher(this);
            _playerAnimatorController.Dash();
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
            _mover = new Mover(this);
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
        }

        public void OnObjectSpawn()
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

        private void OnDrawGizmos()
        {
            var position = this.transform.position;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(position + _playerParameters.GrounderPosition, _playerParameters.GrounderSizes);

            Gizmos.color = new Color(0f, 0.5f, 0f, 1f);
            Gizmos.DrawWireCube(position + _playerParameters.NearGroundGrounderPosition, _playerParameters.NearGroundGrounderSizes);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.BasicAttackCenter, _playerParameters.BasicAttackRadius);
            var leftAttackCenter = (Vector3) _playerParameters.BasicAttackCenter;
            leftAttackCenter.x *= -1;
            Gizmos.DrawWireSphere(position + leftAttackCenter, _playerParameters.BasicAttackRadius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.BasicUpAttackCenter, _playerParameters.BasicUpAttackRadius);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.BasicDownAttackCenter, _playerParameters.BasicDownAttackRadius);
            
            // Gizmos.color = new Color(1f, 0.5f, 0f, 1f);
            // Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.StrongAttackCenter, _playerParameters.StrongAttackRadius);
            // leftAttackCenter = (Vector3) _playerParameters.StrongAttackCenter;
            // leftAttackCenter.x *= -1;
            // Gizmos.DrawWireSphere(position + leftAttackCenter, _playerParameters.StrongAttackRadius);
        }
    }

    public enum Directions
    {
        Right = 1,
        Up,
        Left,
        Down,
    }
}
