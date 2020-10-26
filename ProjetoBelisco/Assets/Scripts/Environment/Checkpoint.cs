using UnityEngine;

namespace Belisco
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : PlayerRespawner
    {
        private bool _hasBeenChecked;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasBeenChecked) return;

            SetToCurrentRespawner();
            _hasBeenChecked = true;
        }
    }
}