using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Belisco
{
    public class Bootstrapper
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            RewiredPlayerInput inputSystem = Object.FindObjectOfType<RewiredPlayerInput>();
            if (inputSystem != null)
                return;

            Addressables.InstantiateAsync("[INPUT SYSTEM].prefab");
        }
    }
}