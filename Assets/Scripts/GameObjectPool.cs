using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _prefabs;
    private GameObject[] _prefabArr;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Start() {
        ObjectToArr();
    }
    
// Dequeues GameObject from the _pool
    public GameObject Get() {
        if(_pool.Count == 0)
            AddToPool(1);
        
        return _pool.Dequeue();
    }

// Enqueues GameObject back into _pool
    public void ReturnToPool(GameObject objectReturning) {
        objectReturning.SetActive(false);
        _pool.Enqueue(objectReturning);
    }

// Creates new objects to add to the _pool, enqueues
    public void AddToPool(int count) {
        for(int i = 0; i < count; i++) {
            GameObject newObject;
            if(_prefabs.transform.childCount > 1) {
                newObject = Instantiate(_prefabArr[Random.Range(0, _prefabs.transform.childCount)]);
            } else newObject = Instantiate(_prefabs);
            
            newObject.SetActive(true);
            _pool.Enqueue(newObject);
        }
    }

// Changes prefabs object to an array of _prefabs
    private void ObjectToArr() {
        if(_prefabs.transform.childCount > 1){
            _prefabArr = new GameObject[_prefabs.transform.childCount];

            for(int i = 0; i < _prefabs.transform.childCount; i++) {
                _prefabArr[i] = _prefabs.transform.GetChild(i).gameObject;
            }
        }  
    }
}
