using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (GameObject.FindObjectOfType<RewiredPlayerInput>() != null)
                return;
            
            var inputGameObject = new GameObject("[INPUT SYSTEM]");
            inputGameObject.AddComponent<RewiredPlayerInput>();
            GameObject.DontDestroyOnLoad(inputGameObject);
        }
    }
}
