using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Grounder 
    {
        private readonly Transform _playerTransform;

        private readonly Vector3 _grounderPosition;
        private readonly Vector2 _grounderSizes;
        private readonly LayerMask _layerMask;

        public bool IsGrounded => Physics2D.OverlapBox(_playerTransform.position + _grounderPosition, _grounderSizes, 0f, _layerMask);

        public Grounder(Player player)
        {
            _layerMask = player.PlayerParameters.GrounderLayerMask;
            _playerTransform = player.transform;
            _grounderPosition = player.PlayerParameters.GrounderPosition;
            _grounderSizes = player.PlayerParameters.GrounderSizes;
        }
    }
}