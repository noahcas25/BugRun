using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
// Variables
    [SerializeField]
    private GameObject prefabs;
    private GameObject[] prefabArr;

    public static GameObjectPool Instance { get; private set;}

    private Queue<GameObject> pool = new Queue<GameObject>();

// Start is called before the first frame update
    private void Start() {
        ObjectToArr();
    }

    private void Awake() {
        Instance = this;
    }

// Dequeues GameObject from the pool
    public GameObject Get() {
        if(pool.Count == 0) {
            AddToPool(1);
        }
        
        return pool.Dequeue();
    }

// Enqueues GameObject back into pool
    public void ReturnToPool(GameObject objectReturning) {
        objectReturning.SetActive(false);
        pool.Enqueue(objectReturning);
    }

// Creates new objects to add to the pool, enqueues
    public void AddToPool(int count) {
        for(int i = 0; i < count; i++) {
            GameObject newObject;
            if(prefabs.transform.childCount > 1) {
                newObject = Instantiate(prefabArr[Random.Range(0, prefabs.transform.childCount)]);
            } else newObject = Instantiate(prefabs);
            
            newObject.SetActive(true);
            pool.Enqueue(newObject);
        }
    }

// Changes prefabs object to an array of prefabs
    private void ObjectToArr() {
        if(prefabs.transform.childCount > 1){
            prefabArr = new GameObject[prefabs.transform.childCount];

            for(int i = 0; i < prefabs.transform.childCount; i++) {
                prefabArr[i] = prefabs.transform.GetChild(i).gameObject;
            }
        }  
    }
}
