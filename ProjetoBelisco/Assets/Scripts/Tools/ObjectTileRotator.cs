#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    public class ObjectTileRotator : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons] private Directions _facingDirection = Directions.Up;

        private void OnValidate()
        {
            switch (_facingDirection)
            {
                case Directions.Up:
                    transform.rotation = Quaternion.Euler(0, 0, 0f);
                    break;
                case Directions.Left:
                    transform.rotation = Quaternion.Euler(0, 0, 90f);
                    break;
                case Directions.Down:
                    transform.rotation = Quaternion.Euler(0, 0, 180f);
                    break;
                case Directions.Right:
                    transform.rotation = Quaternion.Euler(0, 0, 270f);
                    break;
            }
        }
    }
}
#endif