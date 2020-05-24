﻿using UnityEngine;

namespace GameScripts.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class Checkpoint : PlayerRespawner
    {
        private bool _hasBeenChecked = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasBeenChecked) return;
        
            base.SetToCurrentRespawner();
            _hasBeenChecked = true;
        }
    }
}
