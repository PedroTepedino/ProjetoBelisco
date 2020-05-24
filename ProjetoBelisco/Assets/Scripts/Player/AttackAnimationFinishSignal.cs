using UnityEngine;

namespace GameScripts.Player
{
    public class AttackAnimationFinishSignal : MonoBehaviour
    {
        public void OnAnimationFinish()
        {
            SendMessageUpwards("LetAttackAgain");
        }
    }
}
