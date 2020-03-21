using UnityEngine;

public class PlayerPawStorage : MonoBehaviour
{
    public static int PawCount { get; private set; } = 0;

    public static System.Action<int> OnUpdatePawValue;

    public static void AddPaws(int quantity = 1)
    {
        if (quantity > 0)
        {
            PawCount += quantity;
            OnUpdatePawValue?.Invoke(PawCount);
            Debug.Log(PawCount);
        }
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
