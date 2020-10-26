using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class ArrowShooter : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons] private Directions _shootingDirectionEnum = Directions.Right;

        [SerializeField] private LayerMask _layersToCheck;
        [SerializeField] private float _colliderRadius = 1f;
        [SerializeField] private float _maxTravelDistance = 10f;

        [SerializeField] private float _timeBetweenShots = 1f;

        private readonly RaycastHit2D[] _colliders = new RaycastHit2D[5];
        private Vector2 _shootingDirection;
        private float _timer;

        private void Awake()
        {
            _shootingDirection = HelperFunctions.GetDirectionVector(_shootingDirectionEnum);
        }

        private void Update()
        {
            if (_timer <= 0)
            {
                var collisions = Physics2D.CircleCastNonAlloc(transform.position, _colliderRadius, _shootingDirection,
                    _colliders, _maxTravelDistance, _layersToCheck);
                for (var i = 0; i < collisions; i++)
                    if (_colliders[i].collider.gameObject.CompareTag("Player"))
                        Shoot();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                transform.position +
                (Vector3) (HelperFunctions.GetDirectionVector(_shootingDirectionEnum) * _maxTravelDistance),
                _colliderRadius);
            Gizmos.DrawLine(transform.position,
                transform.position +
                (Vector3) (HelperFunctions.GetDirectionVector(_shootingDirectionEnum) * _maxTravelDistance));
        }

        private void Shoot()
        {
            _timer = _timeBetweenShots;
            Pooler.Instance.SpawnFromPool("Arrow", transform.position,
                HelperFunctions.GetDirectionAngle(_shootingDirectionEnum));
        }
    }
}