using System.Collections;
using NSubstitute;
using Belisco;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace a_player
{
    public static class Helpers
    {
        public static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            return player;
        }

        // public static PawInventory GetPawInventory()
        // {
        //     PawInventory inventory = GameObject.FindObjectOfType<PawInventory>();
        //     return inventory;
        // }

        public static IEnumerator LoadPlayerScene()
        {
            var operation = SceneManager.LoadSceneAsync("PlayerTests");
            while (!operation.isDone)
                yield return null;
        }

        public static IEnumerator LoadItemScene()
        {
            var operation = SceneManager.LoadSceneAsync("ItemTests");
            while (!operation.isDone)
                yield return null;
        }

        public static IEnumerator LoadEnemyScene()
        {
            var operation = SceneManager.LoadSceneAsync("EnemyTests");
            while (!operation.isDone)
                yield return null;
        }

        public static void SetupInputSystem()
        {
            RewiredPlayerInput.Instance = Substitute.For<IPlayerInput>();
        }

        // public static Paw GetPaw()
        // {
        //     return GameObject.FindObjectOfType<Paw>();
        // }
    }
}
