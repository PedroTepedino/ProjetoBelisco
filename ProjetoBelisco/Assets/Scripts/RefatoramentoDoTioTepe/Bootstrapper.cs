using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RefatoramentoDoTioTepe
{
    public class Bootstrapper
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            var inputSystem = GameObject.FindObjectOfType<RewiredPlayerInput>();
            if (inputSystem != null)
                return;

            Addressables.InstantiateAsync("[INPUT SYSTEM].prefab");
        }
    }
}
