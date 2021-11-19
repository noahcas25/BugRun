using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawnerScript : MonoBehaviour
{


     [SerializeField]
    private GameObjectPool poolCoins;

    [SerializeField]
    private GameObjectPool poolFood;

    [SerializeField]
    private GameObject player;

    GameObject newCollectible;

    // Spawn boolean
    private bool spawn = true;
    private static int counter = 0;

    // private Vector3 start;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
            lastPosition.x = (float) -27.5;
            lastPosition.y = (float) 0.45;

            poolCoins.AddToPool(15);
            poolFood.AddToPool(10);

    }

    // Update is called once per frame
    void Update()
    {
        if(spawn) {
            SpawnCollectible();
        }     
    }

    private void SpawnCollectible() {
        StartCoroutine(SpawnTimer());
        
        lastPosition.z = player.transform.position.z;

        if(counter%25==0) {
           newCollectible = poolFood.Get();
        } else newCollectible = poolCoins.Get();


         counter++;

       newCollectible.SetActive(true);

        switch( Random.Range(0,3)) {
            case 0:
                newCollectible.transform.position = lastPosition + transform.forward * (float)75.5 + transform.right * (float)-1.25;
            break;

            case 1:
                newCollectible.transform.position = lastPosition + transform.forward * (float)75.5;
            break;

            case 2:
                newCollectible.transform.position = lastPosition + transform.forward * (float)75.5 + transform.right * (float)1.25;
            break;
        }
    }

    IEnumerator SpawnTimer() {

        spawn = false;
        yield return new WaitForSeconds((float) 11/player.GetComponent<PlayerControllerScript>().walkSpeed);
        spawn = true;
    }
}
