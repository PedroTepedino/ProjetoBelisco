using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Rewired;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField][AssetsOnly][InlineEditor(InlineEditorObjectFieldModes.Hidden)] private PlayerParameters _playerParameters;
        private IMover _mover;

        public PlayerParameters PlayerParameters => _playerParameters;

        private void Awake()
        {
            _mover = new Mover(this);
        }

        private void Update()
        {
            _mover.Tick();
        }

        private void OnValidate()
        {
            if (_playerParameters == null)
                _playerParameters = Resources.Load<PlayerParameters>("ScriptableObjects/DefaultPlayerParameters"); 
        }
    }
}
