using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

/* Class: Pool
 * Class that descriobes a type of objects to be stored in a pool.
 * About::
 * Every parameter in the class is public, to facilitate access.
 */
[System.Serializable]
public class Pool
{
    /* Variables: 
     * prefab - The _prefab_ of the object to be stored.
     * tag - The String that stores the name to be called to spawn the object.
     * size - The initial size this pool should have.
     * fixSize - Bool indication whether or not this pool should expand or not.
     */
    [HorizontalGroup("Non-Listed", width: 80)]
    [PreviewField(80, ObjectFieldAlignment.Left), HideLabel, AssetsOnly] 
    public GameObject prefab;

    [HorizontalGroup("Non-Listed")]
    [BoxGroup("Non-Listed/Properties", false)]
    public string tag;

    [HorizontalGroup("Non-Listed")]
    [BoxGroup("Non-Listed/Properties", false)]
    public int size;

    [HorizontalGroup("Non-Listed")]
    [BoxGroup("Non-Listed/Properties", false)]
    public bool fixSize;
}

/* Class: ObjectPooler
 * Class that stores and manages the spawning and creation of the designated objects.
 */
public class ObjectPooler : MonoBehaviour
{
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


    #region Singleton
    /* Variable: Instance
    * The instance of the Singleton.
    */
    public static ObjectPooler Instance = null;
    
    /* Function: Awake
     * Sets the singleton to be the one and only game object existing in a scene.
     */
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (this.transform.parent != null)
            {
                this.transform.parent = null;
            }
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

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
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
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

    public GameObject SpawnFromPool(string tag)
    {
        if (!KeyExists(tag))
        {
            return null;
        }

        GameObject objectToSpawn = Spawn(tag);

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        else
        {
            Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!KeyExists(tag))
        {
            return null;
        }

        GameObject objectToSpawn = Spawn(tag);

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        else
        {
            Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(string tag, Transform trans, bool parent = false)
    {
        if(!KeyExists(tag))
        {
            return null;
        }

        GameObject objectToSpawn = Spawn(tag);

        objectToSpawn.transform.position = trans.position;
        objectToSpawn.transform.rotation = trans.rotation;

        if (parent)
        {
            objectToSpawn.transform.parent = trans;
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
        else
        {
            Debug.LogError("Object does not contain Interface - " + tag, objectToSpawn);
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public bool KeyExists(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Object pooler does not contain tag! - " + tag , this.gameObject);
            return false;
        }
        else
        {
            return true;
        }
    }

    private GameObject Spawn(string tag)
    {
        GameObject aux;
        
        if ((poolDictionary[tag].Count == 0 || poolDictionary[tag].Peek().activeInHierarchy) && !sizeDictionary[tag]) 
        {
            if (prefabDictionary.ContainsKey(tag))
            {
                aux = Instantiate(prefabDictionary[tag]);
                DontDestroyOnLoad(aux);
            }
            else
            {
                aux = null;
                Debug.LogError("Prefab Dictionary does not contain tag! - " + tag, this.gameObject);
            }
        }
        else
        {
            aux = poolDictionary[tag].Dequeue();
            aux.SetActive(true);
        }
        
        return aux;
    }

    public int CountInstaces(string tag)
    {
        if (!KeyExists(tag))
        {
            return -1;
        }

        return poolDictionary[tag].Count;
    }

    public int CountSpawnedInstances(string tag)
    {
        if (!KeyExists(tag))
        {
            return -1;
        }

        int aux = 0;

        foreach(GameObject obj in poolDictionary[tag])
        {
            if (obj.activeInHierarchy)
            {
                aux++;
            }
        }

        return aux;
    }
}