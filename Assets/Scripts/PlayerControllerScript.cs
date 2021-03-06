using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
// Variables
    [SerializeField] 
    private GameObjectPool coinPool, foodPool;
    [SerializeField] 
    private int jumpForce = 5;
    [SerializeField]
    private float walkSpeed = 8;
    [SerializeField] 
    private Camera camera;
    [SerializeField] 
    private Canvas gameController;

    // Player Data
    private Rigidbody rb;
    private GameObject playerMesh;
    private int lives = 3;
    private bool canWalk = true;
    private bool canJump = true;
    private bool canGetHit = true;

    // Used for Lerping player
    private bool shouldLerp = false;
    private float movePosition;
    private float endPos = (float) -27.5;
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

// Update is called once per frame
    void Update()
    {
        if(canWalk)
            Walk();
        if(shouldLerp) 
            LerpPlayer();
    }

// Moves the player and camera at a constant rate based on the walk speed 
    private void Walk() {
            transform.position += transform.forward * walkSpeed * Time.deltaTime;
            camera.transform.position = transform.position + transform.forward * (float)(-8) + transform.up * (float)5;
    }

// Function used to move players position over time
    private void LerpPlayer() {
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

// Function used to position the player and camera during gameplay
    public void Move(string direction) {
        switch(direction){

            case"Left": 
                if(endPos > -28) {
                    movePosition = (float) -1.25;
                    xPos = transform.position.x;
                    endPos = truePosTracker + movePosition;
                    truePosTracker = endPos;
                    shouldLerp = true;
                     StartCoroutine(MoveTimer(-1));
                }
            break;

            case "Right": 
                if(endPos < -27) {
                    movePosition = (float) 1.25;
                    xPos = transform.position.x;
                    endPos = truePosTracker + movePosition;
                    truePosTracker = endPos;
                    shouldLerp = true;
                     StartCoroutine(MoveTimer(1));
                }
            break;

            case "Up":
                Jump();
            break;

            case "Down":
                if(!canJump)
                    rb.AddForce(0, -jumpForce, 0, ForceMode.Impulse);
            break;
        }
    }

// Adds a force to the Players RB to jump 
    public void Jump() {
        if(canJump){
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            StartCoroutine(JumpTimer());
        }
    }

// Function for player damage
    private void PlayerHit() {
        lives--;
        gameController.GetComponent<GameControllerScript>().PlayerHit();

        if(lives==0) {
            gameController.GetComponent<GameControllerScript>().PlayerDied();
            canGetHit = false;
        }
        else if (walkSpeed > 8.0)
            walkSpeed-=(float)0.5;
    }

// Getters and Setters
    public int GetLives() {
        return lives;
    }

    public float GetWalkSpeed() {
        return walkSpeed;
    }

    public GameObject GetPlayerMesh() {
        return playerMesh;
    }

    public void SetPlayerMesh(GameObject mesh) {
        playerMesh = mesh;
    }

    public void IncreaseWalkSpeed() {
        walkSpeed+=(float)0.5;
    }

    public void setCanWalk(bool value) {
        canWalk = value;
    }

// Timers for jumping, hitability, and when moving
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

    private IEnumerator MoveTimer(int value) {
       playerMesh.transform.Rotate(0, value * 30, 0);
       yield return new WaitForSeconds((float) 0.1);
       playerMesh.transform.Rotate(0, -value * 30, 0);
    }
    
    
// Triggers if player collides with something
    private void OnTriggerEnter(Collider other) {
        if((other.CompareTag("Trap") || other.CompareTag("TrapOverlay")) && canGetHit == true) {
           StartCoroutine(HitTimer());
           PlayerHit();
        }
        else if(other.CompareTag("Food")) {
            foodPool.ReturnToPool(other.gameObject);
            gameController.GetComponent<GameControllerScript>().CollectibleObtained(10);
        }
        else if(other.CompareTag("Coin")) {
            coinPool.ReturnToPool(other.gameObject);
            gameController.GetComponent<GameControllerScript>().CollectibleObtained(1);
        }
        else if(other.CompareTag("SceneryTrigger")) {
            gameController.GetComponent<GameControllerScript>().SpawnScenery();
            other.enabled = false;
        }
    }
}
