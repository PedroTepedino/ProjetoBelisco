using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RefatoramentoDoTioTepe
{
    public class LoadMenu : MonoBehaviour
    {
        public void Load()
        {
            SceneManager.LoadScene("Menu");
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                Load();
            }
        }
    }
}