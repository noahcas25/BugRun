using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectPool _poolCoins, _poolFood;
    [SerializeField] private BugPlayer _player;

    private bool _shouldSpawn = true;
    private static int _counter = 0;
    private Vector3 _lastPosition;

    private void Start() {
        _lastPosition.x = (float) -27.5;
        _lastPosition.y = (float) 0.45;
        _poolFood.AddToPool(10);
    }

    private void Update() {
        if(_shouldSpawn)
            SpawnCollectible();
    }

// Obtains Collectible from gameObjectPool and places it at a randomly position (1-3) ahead of the _player
    private void SpawnCollectible() {
        StartCoroutine(SpawnTimer());
        
        _lastPosition.z = _player.transform.position.z;
        GameObject newCollectible;

        if(_counter%25==0)
           newCollectible = _poolFood.Get(); 
        else newCollectible = _poolCoins.Get();

         _counter++;

       newCollectible.SetActive(true);

        switch(Random.Range(0,3)) {
            case 0: newCollectible.transform.position = _lastPosition + transform.forward * (float)85.5 + transform.right * (float)-1.25;
            break;
            case 1: newCollectible.transform.position = _lastPosition + transform.forward * (float)85.5;
            break;
            case 2: newCollectible.transform.position = _lastPosition + transform.forward * (float)85.5 + transform.right * (float)1.25;
            break;
        }
    }

// SpawnTimer depending on _players speed
    IEnumerator SpawnTimer() {
        _shouldSpawn = false;
        yield return new WaitForSeconds((float) 11/_player.GetWalkSpeed());
        _shouldSpawn = true;
    }
}
