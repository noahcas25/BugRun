using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{

     [SerializeField]
    private GameObjectPool poolScenery;

    public GameObject floorPane;
    private GameObject scenery;
    private Vector3 lastScenePosition;
    private static int count = 0;


    void Start() {
         scenery = transform.GetChild(1).gameObject;
         lastScenePosition = transform.GetChild(0).transform.position;

    }

    public void SpawnScenery() {
        Debug.Log("Worked");
        GameObject nextScenery = Instantiate(scenery);
        nextScenery.transform.position = lastScenePosition;
        nextScenery.SetActive(true);
        nextScenery.transform.position += transform.forward * (float)120;
        lastScenePosition = nextScenery.transform.position;
        count++;

        if(count%15==0) {
            GameObject nextFloorPane = Instantiate(floorPane);
            nextFloorPane.transform.position += transform.forward * (float)1000;
            // StartCoroutine(FloorTimer(floorPane));
            // nextFloorPane = floorPane;

        }
    }

    IEnumerator FloorTimer(GameObject floor) {
        yield return new WaitForSeconds((float) 20);
        floor.SetActive(false);
    }
}
