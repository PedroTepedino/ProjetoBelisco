using System;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Bootstrapper
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Initialize()
        {
            if (RewiredPlayerInput.Instance != null)
                return;
            
            var inputGameObject = new GameObject("[INPUT SYSTEM]");
            inputGameObject.AddComponent<RewiredPlayerInput>();
            GameObject.DontDestroyOnLoad(inputGameObject);
        }
    }
}
