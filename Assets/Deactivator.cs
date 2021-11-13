using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + transform.forward * (float)-5.5;
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Floor")){
            other.gameObject.SetActive(false);
            Debug.Log("Hit");
        }
    }
}
