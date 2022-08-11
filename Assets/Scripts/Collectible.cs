using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private bool _isFood;
    
    private float _rotationY, _rotationZ = 0;

    private void Start() {
        if(_isFood) _rotationY = 45;
        else _rotationZ = 45;
    }

    private void Update() => transform.Rotate(new Vector3(0, _rotationY, _rotationZ) * Time.deltaTime * 2);
}