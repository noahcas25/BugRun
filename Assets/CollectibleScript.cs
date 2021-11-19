using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{

    public bool isFood;
    private float rotationY = 0;
    private float rotationZ = 0;

    void Start() {
        if(isFood) rotationY = 45;
        else rotationZ = 45;
    }
    // Update is called once per frame
    void Update()
    {
       transform.Rotate(new Vector3(0, rotationY, rotationZ) * Time.deltaTime * 2);
    }
}