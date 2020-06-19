using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;
using RefatoramentoDoTioTepe;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class a_player
    {
        [SetUp]
        public void setup()
        {
            if (RewiredPlayerInput.Instance == null)
            {
                Bootstrapper.Initialize();
            }
            
            RewiredPlayerInput.Instance = Substitute.For<IPlayerInput>();
        }
        
        [UnityTest]
        public IEnumerator with_positive_horizontal_input()
        { 
            yield return LoadMovementScene();

            var player = GetPlayer();

            RewiredPlayerInput.Instance.Horizontal.Returns(1f);

            float initialPosition = player.transform.position.x;
            yield return new WaitForSeconds(5f);

            float finalPosition = player.transform.position.x;
            
            Assert.Greater(initialPosition, finalPosition);
        }

        private static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            return player;
        }

        private static IEnumerator LoadMovementScene()
        {
            var operation = SceneManager.LoadSceneAsync("MovementTests");
            while (!operation.isDone)
                yield return null;
        }
    }
}
