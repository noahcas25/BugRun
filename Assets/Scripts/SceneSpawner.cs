using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectPool _poolScenery;
    [SerializeField] private GameObject _floorPane;

    private Vector3 _floorPosition, _lastScenePosition;
    private static int _count = 0;

    private void Awake() {
         _lastScenePosition = transform.GetChild(0).transform.position;
         _floorPosition = _floorPane.transform.position;
    }

    public void SpawnScenery() {
        GameObject nextScenery = _poolScenery.Get();
        nextScenery.transform.position = _lastScenePosition;
        nextScenery.SetActive(true);
        nextScenery.GetComponent<BoxCollider>().enabled = true;
        nextScenery.transform.position += transform.forward * (float)145;
        _lastScenePosition = nextScenery.transform.position;
        _count++;

        if(_count%12==0) {
            GameObject nextFloorPane = Instantiate(_floorPane);
            nextFloorPane.transform.position = _floorPosition + transform.forward * (float)2000;
            _floorPosition = nextFloorPane.transform.position;
        }
    }
}
