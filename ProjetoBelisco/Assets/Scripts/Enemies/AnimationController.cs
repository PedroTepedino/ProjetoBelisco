// using UnityEngine;

// namespace GameScripts.Enemies
// {
//     public class AnimationController : MonoBehaviour
//     {
//         private Animator _animator;
//         private StateMachine.StateMachine _stateMachine;
//         private Rigidbody2D _rigidBody;
//         private Attack _attack;
//         private Life _life;

//         [SerializeField] private bool _useVelocity = true;
//         [SerializeField] private bool _useAttackMode = true;

//         private void Awake()
//         {
//             _animator = this.GetComponentInChildren<Animator>();
//             _stateMachine = this.GetComponent<StateMachine.StateMachine>();
//             _rigidBody = this.GetComponent<Rigidbody2D>();
//             _attack = this.GetComponent<Attack>();
//             _life = this.GetComponent<Life>();

//             _attack.OnAttack += ListenAttack;
//             _life.OnEnemyDamage += ListenDamage;
//         }

//         private void OnDestroy()
//         {
//             _attack.OnAttack -= ListenAttack;
//             _life.OnEnemyDamage -= ListenDamage;
//         }

//         private void Update()
//         {
//             if (_useVelocity)
//             {
//                 _animator.SetFloat("Velocity", Mathf.Abs(_rigidBody.velocity.x));
//             }

//             if (_useAttackMode)
//             {
//                 _animator.SetBool("AttackModeOn", _attack.IsInRange);
//             }
//             Debug.Log(_attack.IsInRange);
//         }
    
//         private void ListenAttack(int index)
//         {
//             _animator.SetInteger("AttackIndex", index);
//             _animator.SetTrigger("Attack");
//         }

//         private void ListenDamage(int damage, int maxHealth)
//         {
//             _animator.SetTrigger("Hit");
//         }
//     }
// }
