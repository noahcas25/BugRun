using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
// Variables
     [SerializeField]
    private GameObjectPool poolScenery;
    [SerializeField]
    private GameObject floorPane;

    private Vector3 floorPosition;
    private Vector3 lastScenePosition;
    private GameObject scenery;
    private static int count = 0;

// Start is called before the first frame update
    void Start() {
         scenery = transform.GetChild(1).gameObject;
         lastScenePosition = transform.GetChild(0).transform.position;
         floorPosition = floorPane.transform.position;
    }

// Spawns scenery from gameObjectPool
    public void SpawnScenery() {
        GameObject nextScenery = poolScenery.Get();
        nextScenery.transform.position = lastScenePosition;
        nextScenery.SetActive(true);
        nextScenery.GetComponent<BoxCollider>().enabled = true;
        nextScenery.transform.position += transform.forward * (float)145;
        lastScenePosition = nextScenery.transform.position;
        count++;

        if(count%12==0) {
            GameObject nextFloorPane = Instantiate(floorPane);
            nextFloorPane.transform.position = floorPosition + transform.forward * (float)2000;
            floorPosition = nextFloorPane.transform.position;
        }
    }

// Timer for deactivating previous flooring
    IEnumerator FloorTimer(GameObject floor) {
        yield return new WaitForSeconds((float) 20);
        floor.SetActive(false);
    }
}
