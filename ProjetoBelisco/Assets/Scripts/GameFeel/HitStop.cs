using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HitStop : MonoBehaviour
{
    [SerializeField] private float _timeStoped = 0.05f;

    private static bool _wating;

    private void Awake()
    {
        PlayerAttackSystem.OnDamage += Stop;
    }

    private void OnDestroy()
    {
        PlayerAttackSystem.OnDamage -= Stop;
    }

    public void Stop()
    {
        if (!_wating)
        {
            Time.timeScale = 0f;
            StartCoroutine(WaitStopEnd(_timeStoped));
        }
    }

    private IEnumerator WaitStopEnd(float time)
    {
        _wating = true;

        yield return new WaitForSecondsRealtime(time);

        Time.timeScale = 1.0f;

        _wating = false;
    }
}
