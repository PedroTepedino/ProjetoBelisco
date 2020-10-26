using UnityEngine.UI;

namespace Belisco
{
    public abstract class AbstractButton<T> : Button where T : AbstractButton<T>
    {
        protected static T _instance;

        public static bool Pressed => _instance != null && _instance.IsPressed();
    }
}