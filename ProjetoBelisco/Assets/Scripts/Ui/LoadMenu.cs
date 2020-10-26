using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    public class LoadMenu : MonoBehaviour
    {
        private void Update()
        {
            if (Input.anyKey) Load();
        }

        public void Load()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}