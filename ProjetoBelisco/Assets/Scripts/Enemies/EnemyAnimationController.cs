using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _animator;
    private StateMachine _stateMachine;
    private Rigidbody2D _rigidBody;
    private EnemyAttack _enemyAttack;
    private EnemyLife _enemyLife;

    [SerializeField] private bool _useVelocity = true;
    [SerializeField] private bool _useAttackMode = true;

    private void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _stateMachine = this.GetComponent<StateMachine>();
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _enemyAttack = this.GetComponent<EnemyAttack>();
        _enemyLife = this.GetComponent<EnemyLife>();

        _enemyAttack.OnAttack += ListenAttack;
        _enemyLife.OnEnemyDamage += ListenDamage;
    }

    private void OnDestroy()
    {
        _enemyAttack.OnAttack -= ListenAttack;
        _enemyLife.OnEnemyDamage -= ListenDamage;
    }

    private void Update()
    {
        if (_useVelocity)
        {
            _animator.SetFloat("Velocity", Mathf.Abs(_rigidBody.velocity.x));
        }

        if (_useAttackMode)
        {
            _animator.SetBool("AttackModeOn", _enemyAttack.IsInRange);
        }
        Debug.Log(_enemyAttack.IsInRange);
    }
    
    private void ListenAttack(int index)
    {
        _animator.SetInteger("AttackIndex", index);
        _animator.SetTrigger("Attack");
    }

    private void ListenDamage(int damage, int maxHealth)
    {
        _animator.SetTrigger("Hit");
    }
}
