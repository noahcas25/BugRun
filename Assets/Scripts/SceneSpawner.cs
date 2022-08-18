using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectPool _poolScenery;
    [SerializeField] private GameObject _lastScene;

    private Vector3 _lastScenePosition;
    private static int _count = 0;
    
    private void Awake() {
        _lastScenePosition = _lastScene.transform.position;
    }

    public void SpawnScenery() {
        GameObject nextScenery = _poolScenery.Get();
        nextScenery.transform.position = _lastScenePosition;
        nextScenery.SetActive(true);
        nextScenery.GetComponent<BoxCollider>().enabled = true;
        nextScenery.transform.position += transform.forward * (float)200;
        _lastScenePosition = nextScenery.transform.position;
        _count++;
    }
}
