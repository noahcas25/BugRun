using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectPool _trapPool;
    [SerializeField] private BugPlayer _player;

    private Vector3 _lastPosition;
    private bool _shouldSpawn = true;

    private void Start() {
        _lastPosition.x = (float) -27.5;
        _lastPosition.y = (float) 0.05;
    }

    private void Update() {
        if(_shouldSpawn)
            SpawnTrap();       
    }

// Obtains a trap object from the objectPool and places it at a random position ahead of the _player
    private void SpawnTrap() {
        StartCoroutine(SpawnTimer());

        _lastPosition.z = _player.transform.position.z;

        GameObject trapRandom = _trapPool.Get();
        trapRandom.SetActive(true);

    // if childCount for the trap is > 3 -- Its is a saw trap and needs to be placed at a specific position
        if(trapRandom.transform.childCount > 3) {
            _lastPosition.x = (float)-26.71;
            trapRandom.transform.position = _lastPosition + transform.forward * (float)91.5;
            _lastPosition.x = (float)-27.5;
        } else {
            GameObject trapRandom2 = _trapPool.Get();
            trapRandom2.SetActive(true);

            switch( Random.Range(0,3)) {
                case 0:
                    trapRandom.transform.position = _lastPosition + transform.forward * (float)91.5 + transform.right * (float)-1.25;
                    if(Random.Range(0,3) > 0 && trapRandom2.transform.childCount <= 3) 
                        trapRandom2.transform.position = _lastPosition + transform.forward * (float)91.5;
                    else _trapPool.ReturnToPool(trapRandom2);
                break;

                case 1:
                    trapRandom.transform.position = _lastPosition + transform.forward * (float)91.5;
                    _trapPool.ReturnToPool(trapRandom2);
                break;

                case 2:
                    trapRandom.transform.position = _lastPosition + transform.forward * (float)91.5 + transform.right * (float)1.25;
                    if(Random.Range(0,3) > 0  && trapRandom2.transform.childCount <= 3)
                        trapRandom2.transform.position = _lastPosition + transform.forward * (float)91.5;
                    else _trapPool.ReturnToPool(trapRandom2);
                break;
            }
        }
    }

// SpawnTimer depending on _players speed
    IEnumerator SpawnTimer() {
        _shouldSpawn = false;
        yield return new WaitForSeconds((float) 11/_player.GetWalkSpeed());
        _shouldSpawn = true;
    }
}
