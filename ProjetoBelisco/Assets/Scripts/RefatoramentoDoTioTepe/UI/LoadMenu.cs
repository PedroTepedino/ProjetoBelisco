using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace RefatoramentoDoTioTepe
{
    public class LoadMenu : MonoBehaviour
    {
        public void Load()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}