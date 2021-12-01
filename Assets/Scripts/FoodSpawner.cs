using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{

    public GameObject food1;
    public GameObject food2;
    public GameObject food3;
    public GameObject food4;
    public GameObject food5;
    public GameObject food6;
    public GameObject food7;
    public GameObject startFood;
    public GameObject coin;
    private GameObject[] foods;
    // private GameObject[] coins;

    private bool spawn = true;

    private Vector3 start;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
            foods = new GameObject[] {food1, food2, food3, food4, food5, food6, food7};
            // coins = new GameObject[] {coinCopper, coinCopper, coinCopper, coinCopper, coinCopper, coinCopper, coinCopper, coinSilver, coinSilver, coinGold};
            start = startFood.transform.position;
            lastPosition = start;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn) {
            SpawnFood();
        }     
    }

    private void SpawnFood() {
        StartCoroutine(SpawnTimer());

        // GameObject foodRandom = Instantiate(foods[Random.Range(0,7)]);
        
         GameObject newCoin = Instantiate(coin);
    

        newCoin.SetActive(true);
        // foodRandom.SetActive(true);

        switch( Random.Range(0,3)) {
            case 0:
                newCoin.transform.position = lastPosition + transform.forward * (float)12 + transform.right * (float)-1.25;
            break;

            case 1:
                newCoin.transform.position = lastPosition + transform.forward * (float)12;
            break;

            case 2:
                newCoin.transform.position = lastPosition + transform.forward * (float)12 + transform.right * (float)1.25;
            break;
        }
        
        lastPosition = newCoin.transform.position;
        lastPosition.x = (float)-27.5;
        
        // StartCoroutine(DestroyFood(foodRandom));
    }

    IEnumerator SpawnTimer() {

        spawn = false;
        yield return new WaitForSeconds((float) 0.75);
        spawn = true;
    }

    // IEnumerator DestroyFood(GameObject foodRandom) {
    //     yield return new WaitForSeconds((float) 45);
    //     // foodRandom.SetActive(false);
    // }

}
