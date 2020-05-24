using UnityEngine;

namespace GameScripts.Enemies
{
    public class SignalAttackAnimation : MonoBehaviour
    {
        public void OnAttack(int index)
        {
            SendMessageUpwards("ListenAttackFinished", index);
        }
    }
}
