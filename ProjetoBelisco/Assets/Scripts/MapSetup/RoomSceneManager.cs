using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    public class RoomSceneManager : MonoBehaviour
    {
        private RoomSceneManager _instance;

        public RoomSceneManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);   
            }
            else
            {
                Destroy(this.gameObject);
            }

            LoadFirstScene();
        }

        private void LoadFirstScene()
        {
            SceneManager.LoadScene("TESTESTEPE_ROOM_1",LoadSceneMode.Additive);
        }
    }
}