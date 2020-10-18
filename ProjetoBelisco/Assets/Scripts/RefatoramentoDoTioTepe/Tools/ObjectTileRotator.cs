using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class ObjectTileRotator : MonoBehaviour
    {
        [SerializeField] [EnumToggleButtons] private Directions _facingDirection = Directions.Up;

        private void OnValidate()
        {
            switch (_facingDirection)
            {
                case Directions.Up:
                    this.transform.rotation = Quaternion.Euler(0,0,0f);
                    break;
                case Directions.Left:
                    this.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    break;
                case Directions.Down:
                    this.transform.rotation = Quaternion.Euler(0, 0, 180f);
                    break;
                case Directions.Right:
                    this.transform.rotation = Quaternion.Euler(0, 0, 270f);
                    break;
            }
        }
    }
}