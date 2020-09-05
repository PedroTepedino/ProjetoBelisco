using UnityEditor.U2D;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Grounder 
    {
        private readonly Transform _playerTransform;

        private readonly Vector3 _grounderPosition;
        private readonly Vector2 _grounderSizes;
        private readonly LayerMask _layerMask;

        private readonly Vector3 _nearGroundPosition;
        private readonly Vector2 _nearGroundSizes;

        public bool IsGrounded => Physics2D.OverlapBox(_playerTransform.position + _grounderPosition, _grounderSizes, 0f, _layerMask);

        public bool NearGround => Physics2D.OverlapBox(_playerTransform.position + _nearGroundPosition, _nearGroundSizes, 0f, _layerMask);
        
        public Grounder(Player player)
        {
            _layerMask = player.PlayerParameters.GrounderLayerMask;
            _playerTransform = player.transform;
            _grounderPosition = player.PlayerParameters.GrounderPosition;
            _grounderSizes = player.PlayerParameters.GrounderSizes;

            _nearGroundPosition = player.PlayerParameters.NearGroundGrounderPosition;
            _nearGroundSizes = player.PlayerParameters.NearGroundGrounderSizes;
        }
    }
}