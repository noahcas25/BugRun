using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{

    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 endPos;
    private Vector2 distance;


    public GameObject player;
    public float swipeRangeX;
    public float swipeRangeY;
    public float tapRange;

    private bool touchStopped = false;
    private bool canSwipe = false;

    void Start() {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(TouchTimer());
    }

    // Update is called once per frame
    void Update()
    {   
        if(canSwipe) {
            Swipe();
            Keys();
        }
    }

    // Logic for swipe controls

    private void Swipe() {

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            startPos = Input.GetTouch(0).position;

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            currentPos = Input.GetTouch(0).position;

            distance = currentPos - startPos;

            if(!touchStopped) {

                if(Mathf.Abs((float)distance.x) > Mathf.Abs((float)distance.y)) {
                    if(distance.x > swipeRangeX) {
                        player.GetComponent<PlayerControllerScript>().Move("Right");
                        touchStopped = true;
                    }  
                    else if(distance.x < -swipeRangeX) {
                         player.GetComponent<PlayerControllerScript>().Move("Left");
                         touchStopped = true;
                    }
                }
                else if(Mathf.Abs((float)distance.y) > Mathf.Abs((float)distance.x)) {
                    if(distance.y > swipeRangeY) {
                        player.GetComponent<PlayerControllerScript>().Move("Up");
                        touchStopped = true;
                    }
                    else if(distance.y < -swipeRangeY) {
                        player.GetComponent<PlayerControllerScript>().Move("Down");
                        touchStopped = true;
                    }
                }
            }
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            touchStopped = false;
            endPos = Input.GetTouch(0).position;

            distance = endPos - startPos;

            if(Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
                Debug.Log("Tap");
            }
        }
    }

    
    private void Keys() {
        if(Input.GetKeyDown("w")) {
             player.GetComponent<PlayerControllerScript>().Move("Up");
        }
        else if(Input.GetKeyDown("a")) {
             player.GetComponent<PlayerControllerScript>().Move("Left");
        }
        else if(Input.GetKeyDown("s")) {
            player.GetComponent<PlayerControllerScript>().Move("Down");
        }
        else if(Input.GetKeyDown("d")) {
             player.GetComponent<PlayerControllerScript>().Move("Right");
        }
    }
    
    private IEnumerator TouchTimer() {
        yield return new WaitForSeconds(0.1f);
        canSwipe = true;
    }
}
