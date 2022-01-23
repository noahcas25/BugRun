using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawnerScript : MonoBehaviour
{
// Variables
    [SerializeField]
    private GameObjectPool poolCoins, poolFood;
    [SerializeField]
    private GameObject player;

    private bool spawn = true;
    private static int counter = 0;
    private Vector3 lastPosition;

// Start is called before the first frame update
    void Start()
    {
        lastPosition.x = (float) -27.5;
        lastPosition.y = (float) 0.45;
        poolFood.AddToPool(10);
    }

// Update is called once per frame
    void Update()
    {
        if(spawn)
            SpawnCollectible();    
    }

// Obtains Collectible from gameObjectPool and places it at a randomly position (1-3) ahead of the player
    private void SpawnCollectible() {
        StartCoroutine(SpawnTimer());
        
        lastPosition.z = player.transform.position.z;
        GameObject newCollectible;

        // once counter reaches 25 spawn a food collectible
        if(counter%25==0)
           newCollectible = poolFood.Get(); 
        else newCollectible = poolCoins.Get();

         counter++;

       newCollectible.SetActive(true);

        switch(Random.Range(0,3)) {
            case 0: newCollectible.transform.position = lastPosition + transform.forward * (float)85.5 + transform.right * (float)-1.25;
            break;
            case 1: newCollectible.transform.position = lastPosition + transform.forward * (float)85.5;
            break;
            case 2: newCollectible.transform.position = lastPosition + transform.forward * (float)85.5 + transform.right * (float)1.25;
            break;
        }
    }

// SpawnTimer depending on players speed
    IEnumerator SpawnTimer() {
        spawn = false;
        yield return new WaitForSeconds((float) 11/player.GetComponent<PlayerControllerScript>().GetWalkSpeed());
        spawn = true;
    }
}
