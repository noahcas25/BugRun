using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{

     [SerializeField]
    private GameObjectPool poolScenery;
    
    [SerializeField]
    private GameObject floorPane;

    private Vector3 floorPosition;
    private GameObject scenery;
    private Vector3 lastScenePosition;
    private static int count = 0;


    void Start() {
         scenery = transform.GetChild(1).gameObject;
         lastScenePosition = transform.GetChild(0).transform.position;
         floorPosition = floorPane.transform.position;

    }

    public void SpawnScenery() {
        // Debug.Log("Worked");
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

            // StartCoroutine(FloorTimer(floorPane));
            // nextFloorPane = floorPane;
        }
    }

    IEnumerator FloorTimer(GameObject floor) {
        yield return new WaitForSeconds((float) 20);
        floor.SetActive(false);
    }
}
