using UnityEngine;

namespace GameScripts.Player
{
    public class PawStorage : MonoBehaviour
    {
        public static int PawCount { get; private set; } = 0;

        public static System.Action<int> OnUpdatePawValue;

        public static void AddPaws(int quantity = 1)
        {
            if (quantity <= 0) return;
        
            PawCount += quantity;
            OnUpdatePawValue?.Invoke(PawCount);
        }

        public static bool RemovePaws(int quantity = 1)
        {
            int aux = PawCount - quantity;
            if (aux < 0)
            {
                return false;
            }
            else
            {
                PawCount = aux;
                OnUpdatePawValue?.Invoke(PawCount);
                return true;
            }
        }

        public static bool CanRemovePaws(int quantity = 1) => !((PawCount - quantity) < 0);
    }
}
