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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    // Moves the player at a constant rate based on the walk speed 

    private void Walk() {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
        // camera.transform.position = transform.position + transform.forward * (float)(-6.75) + transform.up * (float)4;
        camera.transform.position += transform.forward * walkSpeed * Time.deltaTime;
    }

    public void Move(string direction) {

        switch(direction){

            case"Left": 

                if(transform.position.x > -28) {
                    transform.position = Vector3.Lerp(transform.position, (transform.position += transform.right * (float)-1.25), 100*Time.deltaTime);
                    // transform.position += transform.right * (float)-1.25;
                    // camera.transform.position += transform.right * (float)-1.25;
                }
            break;

            case "Right": 

                if(transform.position.x < -27) {
                     transform.position = Vector3.Lerp(transform.position, (transform.position += transform.right * (float)1.25), 100*Time.deltaTime);
                    // transform.position += transform.right * (float)1.25;
                    // camera.transform.position += transform.right * (float)1.25;
                }
            break;

            case "Up":
                Jump();
            break;

            case "Down":
                if(!canJump) {
                    rb.AddForce(0, -(jumpForce/2), 0, ForceMode.Impulse);
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
