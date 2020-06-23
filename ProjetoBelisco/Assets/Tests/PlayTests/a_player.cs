using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine.TestTools;
using RefatoramentoDoTioTepe;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace a_player
{
    public class Helpers
    {
        public static Player GetPlayer()
        {
            Player player = GameObject.FindObjectOfType<Player>();
            return player;
        }

        public static IEnumerator LoadPlayerScene()
        {
            var operation = SceneManager.LoadSceneAsync("PlayerTests");
            while (!operation.isDone)
                yield return null;
        }

        public static void SetupInputSystem()
        {
            RewiredPlayerInput.Instance = Substitute.For<IPlayerInput>();
        }
    }

    public class input_destroy
    {
        [TearDown]
        public void tear_down()
        {
            GameObject.Destroy(GameObject.FindObjectOfType<RewiredPlayerInput>());
        }
    }

    public class with_positive_horizontal_input : input_destroy
    {
        [UnityTest]
        public IEnumerator moves_right()
        { 
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            float initialPosition = player.transform.position.x;
            
            RewiredPlayerInput.Instance.Horizontal.Returns(1f);
            yield return new WaitForSeconds(0.05f);

            float finalPosition = player.transform.position.x;
            Assert.Greater(finalPosition, initialPosition);
        }
    }
    
    public class with_negative_horizontal_input : input_destroy
    {
        [UnityTest]
        public IEnumerator moves_left()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            float initialPosition = player.transform.position.x;

            RewiredPlayerInput.Instance.Horizontal.Returns(-1f);
            yield return new WaitForSeconds(0.05f);
            
            float finalPosition = player.transform.position.x;
            Assert.Less(finalPosition, initialPosition);
        }
    }
    
    public class with_a_life_system 
    {
        [UnityTest]
        public IEnumerator takes_non_lethal_damage()
        {
            yield return Helpers.LoadPlayerScene();
            var player = Helpers.GetPlayer();
            var lifeSystem = player.LifeSystem;

            lifeSystem.Damage(1);
            yield return null;
            
            Assert.IsTrue(lifeSystem.StillAlive);
        }

        [UnityTest]
        public IEnumerator takes_lethal_damage()
        {
            yield return Helpers.LoadPlayerScene();
            var player = Helpers.GetPlayer();
            var lifeSystem = player.LifeSystem;
            
            Assert.IsTrue(lifeSystem.StillAlive);
            lifeSystem.Damage(lifeSystem.MaxHealth);
            yield return null;
            
            Assert.IsFalse(lifeSystem.StillAlive);
        }

        [UnityTest]
        public IEnumerator disables_object_when_dead()
        {
            yield return Helpers.LoadPlayerScene();
            var player = Helpers.GetPlayer();
            var lifeSystem = player.LifeSystem;
            
            Assert.IsTrue(lifeSystem.StillAlive);
            lifeSystem.Damage(lifeSystem.MaxHealth);
            yield return null;
            
            Assert.IsFalse(lifeSystem.StillAlive);
            Assert.IsFalse(player.gameObject.activeInHierarchy);
        }
    }

    public class jumping : input_destroy
    {
        [UnityTest]
        public IEnumerator keeps_ascending_while_jump_button_is_pressed()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            var playerRigidbody = player.GetComponent<Rigidbody2D>();
            yield return  new WaitForSeconds(0.1f);

            RewiredPlayerInput.Instance.Jump.Returns(true);
            yield return null;
            
            float initialVelocity = playerRigidbody.velocity.y;
            yield return new WaitForSeconds(0.1f);

            var finalVelocity = playerRigidbody.velocity.y;
            Assert.AreEqual(initialVelocity, finalVelocity);
            
            RewiredPlayerInput.Instance.Jump.Returns(false);
            yield return new WaitForSeconds(0.1f);
        }

        [UnityTest]
        public IEnumerator stops_ascending_when_reaches_max_button_hold_time()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            var playerRigidbody = player.GetComponent<Rigidbody2D>();
            yield return  new WaitForSeconds(0.1f);

            RewiredPlayerInput.Instance.Jump.Returns(true);
            yield return null;

            var initialVelocity = playerRigidbody.velocity.y;
            yield return new WaitWhile(() => player.Jumping);
            yield return new WaitForSeconds(0.1f);
            var finalVelocity = playerRigidbody.velocity.y;
            
            Assert.Less(finalVelocity, initialVelocity);
            
            RewiredPlayerInput.Instance.Jump.Returns(false);
            yield return new WaitForSeconds(0.1f);
        }

        [UnityTest]
        public IEnumerator stops_ascending_when_jump_button_is_released()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            var playerRigidbody = player.GetComponent<Rigidbody2D>();
            yield return  new WaitForSeconds(0.1f);

            RewiredPlayerInput.Instance.Jump.Returns(true);
            yield return null;
            
            var initialVelocity = playerRigidbody.velocity.y;
            yield return new WaitForSeconds(0.1f);
            
            RewiredPlayerInput.Instance.Jump.Returns(false);
            yield return new WaitForSeconds(0.1f);
            
            var finalVelocity = playerRigidbody.velocity.y;
            Assert.Less(finalVelocity, initialVelocity);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public class touching_ground : input_destroy
    {
        [UnityTest]
        public IEnumerator starts_ascending_when_jumping_button_pressed()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            float initialPosition = player.transform.position.y;
            
            RewiredPlayerInput.Instance.Jump.Returns(true);
            yield return new WaitForSeconds(0.1f);

            float finalPosition = player.transform.position.y;
            Assert.Greater(finalPosition, initialPosition);
        }
    }

    public class not_touching_ground : input_destroy
    {
        [UnityTest]
        public IEnumerator starts_falling()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            yield return null;

            player.transform.position += Vector3.up;
            yield return null;

            var initialPosition = player.transform.position.y;
            yield return new WaitForSeconds(0.1f);

            var finalPosition = player.transform.position.y;
            Assert.Less(finalPosition, initialPosition);
        }

        [UnityTest]
        public IEnumerator dont_ascend_when_jump_button_is_pressed()
        {
            yield return Helpers.LoadPlayerScene();
            Helpers.SetupInputSystem();
            var player = Helpers.GetPlayer();
            player.transform.position += Vector3.up;
            var initialPosition = player.transform.position.y;
            RewiredPlayerInput.Instance.Jump.Returns(true);

            yield return new WaitForSeconds(0.1f);

            var finalPosition = player.transform.position.y;
            Assert.Less(finalPosition, initialPosition);

            RewiredPlayerInput.Instance.Jump.Returns(false);
            yield return null;
        }
    }
}
