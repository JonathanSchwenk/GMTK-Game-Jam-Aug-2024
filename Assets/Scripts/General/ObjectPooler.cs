using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pool {
    // tag for pool
    public string tag;
    // what objects in the pool
    public GameObject prefab;
    // size of the pool
    public int size;
}

public class ObjectPooler : MonoBehaviour, IObjectPooler
{

    public Dictionary<string, Queue<GameObject>> poolDictionary {get; set;}
    [SerializeField] private List<Pool> pools;
    public List<Pool> Pools {get {return pools;} set {pools = value;}} // So I can turn it into a service and an interface


    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in Pools) {
            Queue<GameObject> objectpool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectpool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) {

        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " does not excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}

