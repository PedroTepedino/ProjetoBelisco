using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAnimationFinishSignal : MonoBehaviour
{
    public void OnAnimationFinish()
    {
        SendMessageUpwards("LetAttackAgain");
    }
}
