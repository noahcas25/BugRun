using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    
    public int walkSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }

    // Function that moves the player at a constant rate

    private void Walk() {
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
    }
}
