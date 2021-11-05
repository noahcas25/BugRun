using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{

    private Vector2 startPos;
    private Vector2 currentPos;
    private Vector2 endPos;


    public GameObject player;
    public float swipeRange;
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

    private void Swipe() {

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            startPos = Input.GetTouch(0).position;

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            currentPos = Input.GetTouch(0).position;

            Vector2 Distance = currentPos - startPos;

            if(!touchStopped) {

                if(Distance.x < -swipeRange) {
                    player.GetComponent<PlayerControllerScript>().Move("Left");
                    touchStopped = true;
                }

                else if(Distance.x > swipeRange) {
                   player.GetComponent<PlayerControllerScript>().Move("Right");
                    touchStopped = true;
                }

                else if(Distance.y > swipeRange) {
                     player.GetComponent<PlayerControllerScript>().Move("Up");
                    touchStopped = true;
                }

                else if(Distance.y < -swipeRange) {
                    Debug.Log("Down");
                    touchStopped = true;
                }
            }
        }


        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            touchStopped = false;
            endPos = Input.GetTouch(0).position;

            Vector2 Distance = endPos - startPos;

            if(Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange) {
                Debug.Log("Tap");
            }
        }
    }





}
