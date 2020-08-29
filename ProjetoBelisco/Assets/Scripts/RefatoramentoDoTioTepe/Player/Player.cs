using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IHittable
    {
        [SerializeField] [AssetsOnly] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;
        private IJumper _jumper;
        private IAttacker _attacker;
        private LifeSystem _lifeSystem;
        private Grounder _grounder;

        public PlayerParameters PlayerParameters => _playerParameters;
        public LifeSystem LifeSystem => _lifeSystem;
        public Grounder Grounder => _grounder;
        public bool Jumping => _jumper.Jumping;

        private void Awake()
        {
            _grounder = new Grounder(this);
            _lifeSystem = new LifeSystem(this);
            _mover = new Mover(this);
            _jumper = new Jumper(this);
            _attacker = new Attacker(this);
        }

        public void Hit(int damage)
        {
            _lifeSystem.Damage(damage);
        }

        private void Update()
        {
            _mover.Tick();
            _jumper.Tick();
        }

        private void OnValidate()
        {
            if (_playerParameters == null)
                _playerParameters = Resources.Load<PlayerParameters>("ScriptableObjects/DefaultPlayerParameters"); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(this.transform.position + _playerParameters.GrounderPosition, _playerParameters.GrounderSizes);
        }

        public void OnObjectSpawn()
        {
            
        }
    }
}
