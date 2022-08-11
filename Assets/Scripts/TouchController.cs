using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    [SerializeField] private BugPlayer _player;
    [SerializeField] private float _swipeRangeX, _swipeRangeY;

    private Vector2 _startPos, _currentPos, _endPos, _distance;
    private bool _touchStopped, _canSwipe;

    private void Start() {
        StartCoroutine(TouchTimer());
    }

    private void Update() {   
        if(_canSwipe)
            Swipe();
    }

// Logic for swipe controls
    private void Swipe() {
    //  Start Touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
            _startPos = Input.GetTouch(0).position;

    //  During Touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            _currentPos = Input.GetTouch(0).position;

            _distance = _currentPos - _startPos;

            if(!_touchStopped) {
                if(Mathf.Abs((float)_distance.x) > Mathf.Abs((float)_distance.y)) {
                    if(_distance.x > _swipeRangeX) {
                        _player.Move("Right");
                        _touchStopped = true;
                    }  
                    else if(_distance.x < -_swipeRangeX) {
                         _player.Move("Left");
                         _touchStopped = true;
                    }
                }
                else if(Mathf.Abs((float)_distance.y) > Mathf.Abs((float)_distance.x)) {
                    if(_distance.y > _swipeRangeY) {
                        _player.Move("Up");
                        _touchStopped = true;
                    }
                    else if(_distance.y < -_swipeRangeY) {
                        _player.Move("Down");
                        _touchStopped = true;
                    }
                }
            }
        }
    //  End Touch
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {

            _touchStopped = false;
            _endPos = Input.GetTouch(0).position;

            _distance = _endPos - _startPos;
        }
    }

// Keyboard Support
    private void Keys() {
        if(Input.GetKeyDown("w")) {
             _player.Move("Up");
        }
        else if(Input.GetKeyDown("a")) {
             _player.Move("Left");
        }
        else if(Input.GetKeyDown("s")) {
            _player.Move("Down");
        }
        else if(Input.GetKeyDown("d")) {
             _player.Move("Right");
        }
    }

    private IEnumerator TouchTimer() {
        yield return new WaitForSeconds(0.1f);
        _canSwipe = true;
    }
}
