using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
// Variables
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float swipeRangeX, swipeRangeY, tapRange;

    private Vector2 startPos, currentPos, endPos, distance;
    private bool touchStopped = false;
    private bool canSwipe = false;

// Start is called before the first frame update
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
    //  Start Touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            startPos = Input.GetTouch(0).position;

    //  During Touch
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
    //  End Touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            touchStopped = false;
            endPos = Input.GetTouch(0).position;

            distance = endPos - startPos;
        }
    }

// Keyboard Support
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
    
// Initial Timer
    private IEnumerator TouchTimer() {
        yield return new WaitForSeconds(0.1f);
        canSwipe = true;
    }
}
