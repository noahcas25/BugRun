using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnerScript : MonoBehaviour
{

    [SerializeField]
    private GameObjectPool pool;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Vector3 lastPosition;

    [SerializeField]
    private bool spawn = true;

    // Start is called before the first frame update
    void Start()
    {
            lastPosition.x = (float) -27.5;
            lastPosition.y = (float) 0.05;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn) {
            SpawnTrap();
        }       
    }

    private void SpawnTrap() {
        StartCoroutine(SpawnTimer());

        lastPosition.z = player.transform.position.z;

        GameObject trapRandom = pool.Get();
        trapRandom.SetActive(true);

    // This is how you know its the Saw
        if(trapRandom.transform.childCount > 3) {
            lastPosition.x = (float)-26.71;
            trapRandom.transform.position = lastPosition + transform.forward * (float)81.5;
            lastPosition.x = (float)-27.5;
        } else {
            GameObject trapRandom2 = pool.Get();
            trapRandom2.SetActive(true);

        switch( Random.Range(0,3)) {
            case 0:
                trapRandom.transform.position = lastPosition + transform.forward * (float)81.5 + transform.right * (float)-1.25;
                if(Random.Range(0,3) > 0 && trapRandom2.transform.childCount <= 3) 
                    trapRandom2.transform.position = lastPosition + transform.forward * (float)81.5;
                else pool.ReturnToPool(trapRandom2);
            break;

            case 1:
                trapRandom.transform.position = lastPosition + transform.forward * (float)81.5;
                pool.ReturnToPool(trapRandom2);
            break;

            case 2:
                trapRandom.transform.position = lastPosition + transform.forward * (float)81.5 + transform.right * (float)1.25;
                if(Random.Range(0,3) > 0  && trapRandom2.transform.childCount <= 3)
                     trapRandom2.transform.position = lastPosition + transform.forward * (float)81.5;
                else pool.ReturnToPool(trapRandom2);
            break;
        }}
    }

    IEnumerator SpawnTimer() {

        spawn = false;
        yield return new WaitForSeconds((float) 11/player.GetComponent<PlayerControllerScript>().walkSpeed);
        spawn = true;
    }
}
