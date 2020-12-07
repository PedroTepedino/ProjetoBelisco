using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace a_player
{
    public class with_an_empty_pawn_inventory
    {
        // [UnityTest]
        // public IEnumerator picks_a_paw_with_different_values([Range(1, 10)] int value)
        // {
        //     yield return Helpers.LoadItemScene();
        //     var inventory = Helpers.GetPawInventory();
        //     var paw = Helpers.GetPaw();
        //     paw.InitializeValue(value);
        //     
        //     Assert.AreEqual(0, inventory.PawBalance);
        //     yield return null;
        //     paw.transform.position = inventory.transform.position;
        //     yield return null;
        //     
        //     Assert.AreEqual(value, inventory.PawBalance);
        // }
    }

    public class with_an_initialized_inventory
    {
        // [UnityTest]
        // public IEnumerator picks_a_paw_with_different_values([Range(1, 10)] int initial_inventory_value, [Range(1, 10)] int paw_value)
        // {
        //     yield return Helpers.LoadItemScene();
        //     var inventory = Helpers.GetPawInventory();
        //     var paw = Helpers.GetPaw();
        //     inventory.Initialize(initial_inventory_value);
        //     paw.InitializeValue(paw_value);
        //     
        //     Assert.AreEqual(initial_inventory_value, inventory.PawBalance);
        //     yield return null;
        //     paw.transform.position = inventory.transform.position;
        //     yield return null;
        //     
        //     Assert.AreEqual(initial_inventory_value + paw_value, inventory.PawBalance);
        // }
    }
}
