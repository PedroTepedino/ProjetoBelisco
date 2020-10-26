using UnityEngine;

namespace Belisco
{
    public static class AttackerTimer
    {
        private static float _timeBetweenAttacks;
        private static float _timer;

        public static bool TimerEnded => _timer <= 0f;

        public static void ResetTimer()
        {
            _timer = _timeBetweenAttacks;
        }

        public static void SetTimer(float timeBetweenAttacks)
        {
            _timeBetweenAttacks = timeBetweenAttacks;
        }

        public static void SubtractTimer()
        {
            _timer -= Time.deltaTime;
        }
    }
}