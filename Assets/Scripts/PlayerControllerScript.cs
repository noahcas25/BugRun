using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    
    public int walkSpeed = 5;
    public int jumpForce = 5;
    public Camera camera;
    public Canvas gameController;

    private Rigidbody rb;
    private int lives = 3;
    private bool canJump = true;
    private bool canGetHit = true;

// Used for Lerping player
    private bool shouldLerp = false;
    private float movePosition;
    private float endPos;
    private float truePosTracker;
    private float xPos;
    private Vector3 pos;
    private Vector3 camPos;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        truePosTracker = transform.position.x;
    }



// xPos = Mathf.Lerp(xPos, xPos-horizontalMovement, Time.deltaTime * 3);


    // Update is called once per frame
    void Update()
    {
        Walk();

        if(shouldLerp) {
            xPos = Mathf.Lerp(xPos, endPos, 20f * Time.deltaTime);
            pos = transform.position;
            camPos = camera.transform.position;

            pos.x = xPos;
            camPos.x = xPos;
            transform.position  = pos;
            camera.transform.position = camPos;
    
            if(Mathf.Abs(transform.position.x - endPos) < 0.01) {
                endPos = (float)Mathf.Round(endPos * 100f) / 100f;
                pos.x = endPos;
                camPos.x = endPos;
                transform.position  = pos;
                camera.transform.position = camPos;
                shouldLerp = false;
            };
        }

        // Walk();
    }

    // Moves the player at a constant rate based on the walk speed 

    private void Walk() {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        // camera.transform.position = transform.position + transform.forward * (float)(-6.75) + transform.up * (float)4;
        camera.transform.position += transform.forward * walkSpeed * Time.deltaTime;
    }

    public void Move(string direction) {
        //  check 
        // endPos = transform.position.x;
        
        // endPos = transform.position.x;

        switch(direction){

            case"Left": 

                if(endPos > -28) {
                    movePosition = (float) -1.25;
                    xPos = transform.position.x;
                    endPos = truePosTracker + movePosition;
                    truePosTracker = endPos;
                    shouldLerp = true;
                }
            break;

            case "Right": 

                if(endPos < -27) {
                    movePosition = (float) 1.25;
                    xPos = transform.position.x;
                    endPos = truePosTracker + movePosition;
                    truePosTracker = endPos;
                    shouldLerp = true;
                    //  transform.position = Vector3.Lerp(transform.position, (transform.position += transform.right * (float)1.25), 1*Time.deltaTime);
                }
            break;

            case "Up":
                Jump();
            break;

            case "Down":
                if(!canJump) {
                    rb.AddForce(0, -jumpForce, 0, ForceMode.Impulse);
                }
            break;
        }
    }

    // Adds a force to the Player RB to jump 

    public void Jump() {
        if(canJump){
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            StartCoroutine(JumpTimer());
        }
    }

    private void PlayerHit() {

        if(walkSpeed > 6)
            walkSpeed--;
            
        lives--;
        gameController.GetComponent<GameControllerScript>().PlayerHit();

        if(lives==0) 
            gameController.GetComponent<GameControllerScript>().PlayerDied();
    }

    public int GetLives() {
        return lives;
    }

    public int GetWalkSpeed() {
        return walkSpeed;
    }

    public void IncreaseWalkSpeed() {
        walkSpeed++;
    }

    // Timer that limits the time between jumps

    private IEnumerator JumpTimer() {
       canJump = false;
       yield return new WaitForSeconds((float) .75);
       canJump = true;
    }

    private IEnumerator HitTimer() {
       canGetHit = false;
       GetComponent<Animator>().enabled = true;

       yield return new WaitForSeconds((float) 2);

       canGetHit = true;
       GetComponent<Animator>().enabled = false;
    }
    

    // Triggers if player collides with something
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Trap") && canGetHit == true) {
           StartCoroutine(HitTimer());
           PlayerHit();
        }

        if(other.CompareTag("Food")) {
            other.gameObject.SetActive(false);
            gameController.GetComponent<GameControllerScript>().FoodObtained();
        }

        if(other.CompareTag("End"))
            gameController.GetComponent<GameControllerScript>().LevelCompleted();
    }
}
