using System;
using GameScripts.PoolingSystem;
using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class ArrowShooter : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons] private Directions _shootingDirectionEnum = Directions.Right;
        private Vector2 _shootingDirection;

        [SerializeField] private LayerMask _layersToCheck;
        [SerializeField] private float _colliderRadius = 1f;
        [SerializeField] private float _maxTravelDistance = 10f;
        
        private RaycastHit2D[] _colliders = new RaycastHit2D[5];

        [SerializeField] private float _timeBetweenShots = 1f;
        private float _timer = 0f;

        private void Awake()
        {
            _shootingDirection = HelperFunctions.GetDirectionVector(_shootingDirectionEnum);
        }

        private void Update()
        {
            if (_timer <= 0)
            {
                int collisions = Physics2D.CircleCastNonAlloc(this.transform.position, _colliderRadius, _shootingDirection, _colliders, _maxTravelDistance, _layersToCheck);
                for (int i = 0; i < collisions; i++)
                {
                    if (_colliders[i].collider.gameObject.CompareTag("Player"))
                    {
                        Shoot();
                    }
                } 
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        private void Shoot()
        {
            _timer = _timeBetweenShots;
            Pooler.Instance.SpawnFromPool("Arrow", this.transform.position,
                HelperFunctions.GetDirectionAngle(_shootingDirectionEnum));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position + (Vector3)(HelperFunctions.GetDirectionVector(_shootingDirectionEnum) * _maxTravelDistance) , _colliderRadius);
            Gizmos.DrawLine(this.transform.position, this.transform.position + (Vector3)(HelperFunctions.GetDirectionVector(_shootingDirectionEnum) * _maxTravelDistance));
        }
    }
}