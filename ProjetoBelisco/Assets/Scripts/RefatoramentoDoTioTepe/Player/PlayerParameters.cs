using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [CreateAssetMenu(menuName = "Player Parameters")]
    public class PlayerParameters : ScriptableObject
    {
        [BoxGroup("Movement")]
        
        [SerializeField] private float _movementSpeed = 5f;
        public float MovementSpeed => _movementSpeed;
    }
}