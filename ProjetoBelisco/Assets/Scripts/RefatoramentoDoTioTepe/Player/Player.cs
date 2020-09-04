using System;
using System.Collections.Generic;
using GameScripts.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IHittable
    {
        [SerializeField] [AssetsOnly] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;
        private IJumper _jumper;
        private List<IAttacker> _attackerList;
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
        public PlayerAnimatorController AnimatorController => _playerAnimatorController;


        private void Awake()
        {
            _grounder = new Grounder(this);
            _lifeSystem = new LifeSystem(this);
            _mover = new Mover(this);
            _jumper = new Jumper(this);
            _playerLocker = new PlayerLocker(this);
            _glider = new Glider(this);
            
            _attackerList = new List<IAttacker>();
            _attackerList.Add(new BasicAttacker(this));
            _attackerList.Add(new StrongAttacker(this));
        }

        public void Hit(int damage)
        {
            _lifeSystem.Damage(damage);
        }

        private void Update()
        {
            if (!_attacking)
            {
                _mover.Tick();

                if (!(_mover is Dasher))
                {
                    _jumper.Tick();
                }
            }

            if (Dasher.CheckDashInput())
            {
                StartDash();
            }

            _attackerList.ForEach(attacker => attacker.Tick());
            
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

        public void CallAttack<T>() where T : IAttacker
        {
            _attackerList.Find(item => item is StrongAttacker)?.Attack();
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

        private void OnDrawGizmos()
        {
            var position = this.transform.position;
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(position + _playerParameters.GrounderPosition, _playerParameters.GrounderSizes);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.StrongAttackCenter, _playerParameters.StrongAttackRadius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(position + (Vector3)_playerParameters.StrongAttackExplosionCenter, _playerParameters.StrongAttackExplosionRadius);
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
