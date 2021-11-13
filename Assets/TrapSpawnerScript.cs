using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawnerScript : MonoBehaviour
{

    public GameObject trap1;
    public GameObject trap2;
    public GameObject trap3;
    public GameObject trap4;
    public GameObject trap5;
    public GameObject trap6;
    // public GameObject trap7;
    public GameObject startTrap;
    private GameObject[] traps;

    private bool spawn = true;
    // private GameObject trapRandom;

    private Vector3 start;
    private Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {
            traps = new GameObject[] {trap1, trap2, trap3, trap4, trap5, trap6};
            start = startTrap.transform.position;
            lastPosition = start;
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

        int rand = Random.Range(0,6);
        GameObject trapRandom = Instantiate(traps[rand]);

        switch( Random.Range(0,3)) {
            case 0:
                trapRandom.transform.position = lastPosition + transform.forward * (float)10 + transform.right * (float)-1.25;
            break;

            case 1:
                trapRandom.transform.position = lastPosition + transform.forward * (float)10;
            break;

            case 2:
                trapRandom.transform.position = lastPosition + transform.forward * (float)10 + transform.right * (float)1.25;
            break;
        }
    
        lastPosition = trapRandom.transform.position;
        lastPosition.x = (float)-27.5;

          if(rand == 3) {
            trapRandom.transform.position = new Vector3((float)-26.75, 0, trapRandom.transform.position.z);
        }
        
        StartCoroutine(DestroyTrap(trapRandom));
    }

    IEnumerator SpawnTimer() {

        spawn = false;
        yield return new WaitForSeconds((float) .75);
        spawn = true;
    }

    IEnumerator DestroyTrap(GameObject trapRandom) {
        yield return new WaitForSeconds((float) 45);
        // trapRandom.SetActive(false);
    }

}
