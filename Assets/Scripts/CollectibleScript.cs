using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
// Variables
    [SerializeField]
    private bool isFood;
    
    private float rotationY = 0;
    private float rotationZ = 0;

// Start is called before the first frame update, changes rotation values depending on the kind of collectible
    void Start() {
        if(isFood) rotationY = 45;
        else rotationZ = 45;
    }
// Update is called once per frame, rotates collectible
    void Update()
    {
       transform.Rotate(new Vector3(0, rotationY, rotationZ) * Time.deltaTime * 2);
    }
}