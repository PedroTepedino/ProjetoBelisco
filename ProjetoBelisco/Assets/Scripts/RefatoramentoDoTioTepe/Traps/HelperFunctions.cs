using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public static class HelperFunctions
    {
        public static Vector2 GetDirectionVector(Directions direction)
        {
            switch (direction)
            {
                case Directions.Down :
                    return Vector2.down;
                case Directions.Right:
                    return Vector2.right;
                case Directions.Up:
                    return Vector2.up;
                case Directions.Left:
                    return Vector2.left;
                default: 
                    return Vector2.zero;
            }
        }

        public static Quaternion GetDirectionAngle(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return Quaternion.Euler(0,0,0f);
                case Directions.Left:
                    return Quaternion.Euler(0, 0, 90f);
                case Directions.Down:
                    return Quaternion.Euler(0, 0, 180f);
                case Directions.Right:
                    return Quaternion.Euler(0, 0, 270f);
                default:
                    return Quaternion.identity;
            }
        }
    }
}