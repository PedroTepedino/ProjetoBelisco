using UnityEngine;

public class EnemySignalAttackAnimation : MonoBehaviour
{
    public void OnAttack(int index)
    {
        SendMessageUpwards("ListenAttackFinished", index);
    }
}
