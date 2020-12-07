using System;
using System.Collections;
using System.Linq;
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
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            SpawnPlayer();
        }

        private void OnEnable()
        {
            //SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            Pooler.Instance.SpawnFromPool("Player", RoomSpawner.CurrentSpawner.transform.position, Quaternion.identity);
        }
    }
}