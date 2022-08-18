using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] private GameObjectPool _trapPool, _coinPool, _foodPool, _scenePool;
    private bool _gameOver;

    private void Start() => GameManager.Instance._gameOverEvent.AddListener(SetGameOver);

    private void SetGameOver(bool value) {
        _gameOver = value;
    }

    
// Triggers that react when deactivator collides with objects
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Floor") || other.CompareTag("Scenery"))
            return;
            
        if(!_gameOver) {
            if(other.CompareTag("Trap")) 
                    _trapPool.ReturnToPool(other.gameObject);
            else if(other.CompareTag("Food"))
                    _foodPool.ReturnToPool(other.gameObject);
            else if(other.CompareTag("Coin"))
                    _coinPool.ReturnToPool(other.gameObject);
            else if(other.CompareTag("TrapOverlay"))
                    _trapPool.ReturnToPool(other.gameObject.transform.parent.gameObject);
            else if(other.CompareTag("EndScene"))
                    _scenePool.ReturnToPool(other.gameObject.transform.parent.parent.gameObject);
            else if(other.CompareTag("SceneryTrigger")) {
                            // Ignore this one
            }
            else other.gameObject.SetActive(false);
        }
    }
}
