using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{

    [SerializeField]
    private GameObject prefabs;
    private GameObject[] prefabArr;

    public static GameObjectPool Instance { get; private set;}

    private Queue<GameObject> pool = new Queue<GameObject>();


    private void Start() {
        ObjectToArr();
    }

    private void Awake() {
        Instance = this;
    }

    public GameObject Get() {
        if(pool.Count == 0) {
            AddToPool(1);
        }
        
        return pool.Dequeue();
    }

    public void ReturnToPool(GameObject objectReturning) {
        objectReturning.SetActive(false);
        pool.Enqueue(objectReturning);
    }

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

    private void ObjectToArr() {
        if(prefabs.transform.childCount > 1){
            prefabArr = new GameObject[prefabs.transform.childCount];

            for(int i = 0; i < prefabs.transform.childCount; i++) {
                prefabArr[i] = prefabs.transform.GetChild(i).gameObject;
            }
        }  
    }
}
