using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Grounder))]
    public class Player : MonoBehaviour
    {
        [SerializeField][AssetsOnly][InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;
        private IJumper _jumper;
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
    }
}
