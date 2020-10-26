using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Belisco
{
    /* Class: Pool
 * Class that descriobes a type of objects to be stored in a pool.
 * About::
 * Every parameter in the class is public, to facilitate access.
 */
    [Serializable]
    public class Pool
    {
        /* Variables: 
     * prefab - The _prefab_ of the object to be stored.
     * tag - The String that stores the name to be called to spawn the object.
     * size - The initial size this pool should have.
     * fixSize - Bool indication whether or not this pool should expand or not.
     */
        [HorizontalGroup("Non-Listed", 80)] [PreviewField(80, ObjectFieldAlignment.Left)] [HideLabel] [AssetsOnly]
        public GameObject prefab;

        [HorizontalGroup("Non-Listed")] [BoxGroup("Non-Listed/Properties", false)]
        public string tag;

        [HorizontalGroup("Non-Listed")] [BoxGroup("Non-Listed/Properties", false)]
        public int size;

        [HorizontalGroup("Non-Listed")] [BoxGroup("Non-Listed/Properties", false)]
        public bool fixSize;
    }

/* Class: ObjectPooler
 * Class that stores and manages the spawning and creation of the designated objects.
 */
    public class Pooler : MonoBehaviour
    {
        /* Variable: Instance
    * The instance of the Singleton.
    */
        public static Pooler Instance;

        /* Variables: 
     * pools - List of Existing Pools.
     * poolDictionary - Dictionary of Queues and tags, ot spawn the gameObjscts.
     * prefabDictionary - Dictionary of tags and Gameobjects, to spawn extras.
     * sizeDictionary - Dictionary to store the maximum size of the pools.
     */
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;
        public Dictionary<string, GameObject> prefabDictionary;
        public Dictionary<string, bool> sizeDictionary;

        /* Function: Awake
     * Sets the singleton to be the one and only game object existing in a scene.
     */
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (transform.parent != null) transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            GameStateMachine.OnGameStateChanged += HandleStateChanged;
        }

        /* Function: Start
     * Creates all objects and associates to the relative pools.
     */
        private void Start()
        {
            if (poolDictionary == null)
            {
                poolDictionary = new Dictionary<string, Queue<GameObject>>();
                prefabDictionary = new Dictionary<string, GameObject>();
                sizeDictionary = new Dictionary<string, bool>();

                foreach (Pool pool in pools)
                {
                    var objectPool = new Queue<GameObject>();

                    for (var i = 0; i < pool.size; i++)
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.SetActive(false);
                        DontDestroyOnLoad(obj);
                        objectPool.Enqueue(obj);
                    }

                    poolDictionary.Add(pool.tag, objectPool);

                    prefabDictionary.Add(pool.tag, pool.prefab);

                    sizeDictionary.Add(pool.tag, pool.fixSize);
                }
            }
        }


        private void OnDestroy()
        {
            GameStateMachine.OnGameStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(IState state)
        {
            if (state is Menu || state is LoadLevel)
                foreach (var keyValuePair in poolDictionary)
                foreach (GameObject gameObject in keyValuePair.Value)
                    gameObject.SetActive(false);
        }

        public GameObject SpawnFromPool(string tag, object[] parameters = null)
        {
            if (!KeyExists(tag)) return null;

            GameObject objectToSpawn = Spawn(tag);

            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

            if (pooledObj != null)
                pooledObj.OnObjectSpawn(parameters);
            else
                Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, object[] parameters = null)
        {
            if (!KeyExists(tag)) return null;

            GameObject objectToSpawn = Spawn(tag);

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

            if (pooledObj != null)
                pooledObj.OnObjectSpawn(parameters);
            else
                Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public GameObject SpawnFromPool(string tag, Transform trans, bool parent = false, object[] parameters = null)
        {
            if (!KeyExists(tag)) return null;

            GameObject objectToSpawn = Spawn(tag);

            objectToSpawn.transform.position = trans.position;
            objectToSpawn.transform.rotation = trans.rotation;

            if (parent) objectToSpawn.transform.parent = trans;

            IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

            if (pooledObj != null)
                pooledObj.OnObjectSpawn(parameters);
            else
                Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);

            poolDictionary[tag].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        public GameObject MoveObjectToPoint(string tag, Transform trans, bool parent = false)
        {
            if (!KeyExists(tag)) return null;

            GameObject obejctToMove = GetNextObject(tag);

            obejctToMove.transform.position = trans.position;
            obejctToMove.transform.rotation = trans.rotation;

            return null;
        }

        public bool KeyExists(string tag)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogError("Object pooler does not contain tag! - " + tag, gameObject);
                return false;
            }

            return true;
        }

        private GameObject Spawn(string tag)
        {
            GameObject aux;

            if ((poolDictionary[tag].Count == 0 || poolDictionary[tag].Peek().activeInHierarchy) &&
                !sizeDictionary[tag])
            {
                if (prefabDictionary.ContainsKey(tag))
                {
                    aux = Instantiate(prefabDictionary[tag]);
                    DontDestroyOnLoad(aux);
                }
                else
                {
                    aux = null;
                    Debug.LogError("Prefab Dictionary does not contain tag! - " + tag, gameObject);
                }
            }
            else
            {
                aux = poolDictionary[tag].Dequeue();
                aux.SetActive(true);
            }

            return aux;
        }

        private GameObject GetNextObject(string tag)
        {
            GameObject aux;

            if ((poolDictionary[tag].Count == 0 || poolDictionary[tag].Peek().activeInHierarchy) &&
                !sizeDictionary[tag])
            {
                if (prefabDictionary.ContainsKey(tag))
                {
                    aux = Instantiate(prefabDictionary[tag]);
                    DontDestroyOnLoad(aux);
                }
                else
                {
                    aux = null;
                    Debug.LogError("Prefab Dictionary does not contain tag! - " + tag, gameObject);
                }
            }
            else
            {
                aux = poolDictionary[tag].Peek();
            }

            return aux;
        }

        public int CountInstaces(string tag)
        {
            if (!KeyExists(tag)) return -1;

            return poolDictionary[tag].Count;
        }

        public int CountSpawnedInstances(string tag)
        {
            if (!KeyExists(tag)) return -1;

            var aux = 0;

            foreach (GameObject obj in poolDictionary[tag])
                if (obj.activeInHierarchy)
                    aux++;

            return aux;
        }
    }
}