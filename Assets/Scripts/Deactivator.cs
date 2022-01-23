using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
// Variables
    [SerializeField]
    private GameObjectPool trapPool, coinPool, foodPool, scenePool;

    private GameObject player;

// Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    
// Triggers that react when deactivator collides with objects
    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Floor") && !other.CompareTag("Scenery")){
            if(other.CompareTag("Trap")) 
                trapPool.ReturnToPool(other.gameObject); 
            else if(other.CompareTag("Food"))
                foodPool.ReturnToPool(other.gameObject);
            else if(other.CompareTag("Coin"))
                coinPool.ReturnToPool(other.gameObject);
            else if(other.CompareTag("TrapOverlay")) 
                trapPool.ReturnToPool(other.gameObject.transform.parent.gameObject);
            else if(other.CompareTag("EndScene"))
                scenePool.ReturnToPool(other.gameObject.transform.parent.gameObject);
            else other.gameObject.SetActive(false);
        }
    }
}
