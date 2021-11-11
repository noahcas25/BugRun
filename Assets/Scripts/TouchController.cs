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

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Swipe();
    }

    // Logic for swipe controls

    private void Swipe() {

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            startPos = Input.GetTouch(0).position;

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            currentPos = Input.GetTouch(0).position;

            distance = currentPos - startPos;

            if(!touchStopped) {

                if(distance.x < -swipeRangeX && distance.y < swipeRangeY) {
                    player.GetComponent<PlayerControllerScript>().Move("Left");
                    touchStopped = true;
                }

                else if(distance.x > swipeRangeX && distance.y < swipeRangeY) {
                   player.GetComponent<PlayerControllerScript>().Move("Right");
                    touchStopped = true;
                }

                else if(distance.y > swipeRangeY) {
                     player.GetComponent<PlayerControllerScript>().Move("Up");
                    touchStopped = true;
                }

                else if(distance.y < -swipeRangeY) {
                   player.GetComponent<PlayerControllerScript>().Move("Down");
                    touchStopped = true;
                }
            }
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            touchStopped = false;
            endPos = Input.GetTouch(0).position;

            distance = endPos - startPos;

            if(Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
                Debug.Log("Tap");
                //  player.GetComponent<PlayerControllerScript>().Move("Up");
            }
        }
    }





}
