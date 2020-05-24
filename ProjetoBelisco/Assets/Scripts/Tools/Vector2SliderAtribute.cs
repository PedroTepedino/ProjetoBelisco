using System;

namespace GameScripts.Tools
{
    public class Vector2SliderAttribute : Attribute
    {
        public float minValue;
        public float maxValue;

        public Vector2SliderAttribute(float min, float max)
        {
            this.minValue = min;
            this.maxValue = max;
        }
    }
}
