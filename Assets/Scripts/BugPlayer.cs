using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BugPlayer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObjectPool _coinPool, _foodPool;
    [SerializeField] private GameObject _playerMesh;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int _jumpForce = 8;
    [SerializeField] private float _walkSpeed = 8;

    private bool _canWalk = true;
    private bool _canJump = true;
    private bool _canGetHit = true;
    private int _lives = 3;
    private Rigidbody _rb;

    // Used for Lerping player
    private bool _shouldLerp = false;
    private float _moveValue, _xMover;
    private float _endXPosition = -27.5f;
    private Vector3 _tempPosition, _tempCamPosition;

    [System.NonSerialized] public UnityEvent<int> _livesChangedEvent;

    public static BugPlayer Instance {get; private set;}

    private void Awake() {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            _playerMesh.SetActive(false);
            _playerMesh = transform.GetChild(PlayerPrefs.GetInt("bugIndex")).gameObject;
            _playerMesh.SetActive(true);
        }

        if(_livesChangedEvent == null)
            _livesChangedEvent = new UnityEvent<int>();
    }

    private void Update() {
        Walk();
        LerpPlayer();
    }

    private void Walk() {
        if(!_canWalk) return;

        transform.position += transform.forward * _walkSpeed * Time.deltaTime;
        _camera.transform.position = transform.position + transform.forward * (float)(-6.5) + transform.up * (float)5;
    }

// Function used to move players position over time
    private void LerpPlayer() {
        if(!_shouldLerp) return;

        _xMover = Mathf.Lerp(_xMover, _endXPosition, 20f * Time.deltaTime);
        _tempPosition = transform.position;
        _tempCamPosition = _camera.transform.position;

        _tempPosition.x = _xMover;
        _tempCamPosition.x = _xMover;
        transform.position  = _tempPosition;
        _camera.transform.position = _tempCamPosition;

        // when lerping is complete
        if(Mathf.Abs(transform.position.x - _endXPosition) < 0.01) {
            _endXPosition = (float)Mathf.Round(_endXPosition * 100f) / 100f;
            _tempPosition.x = _endXPosition;
            _tempCamPosition.x = _endXPosition;
            transform.position  = _tempPosition;
            _camera.transform.position = _tempCamPosition;
            _shouldLerp = false;
        };
    }

    public void Move(string direction) {
        if(!_canWalk || Time.deltaTime == 0) return;

        switch(direction){
            case"Left": 
                if(_endXPosition > -28) {
                    _moveValue = (float) -1.25;
                    _xMover = transform.position.x;
                    _endXPosition += _moveValue;
                    _shouldLerp = true;
                    StartCoroutine(MeshRotate(-1));
                }
            break;

            case "Right": 
                if(_endXPosition < -27) {
                    _moveValue = (float) 1.25;
                    _xMover = transform.position.x;
                    _endXPosition += _moveValue;
                    _shouldLerp = true;
                     StartCoroutine(MeshRotate(1));
                }
            break;

            case "Up":
                Jump();
            break;

            case "Down":
                if(!_canJump)
                    _rb.AddForce(0, -_jumpForce, 0, ForceMode.Impulse);
            break;
        }
    }

    private IEnumerator MeshRotate(int value) {
       _playerMesh.transform.Rotate(0, value * 30, 0);
       yield return new WaitForSeconds((float) 0.1);
       _playerMesh.transform.Rotate(0, -value * 30, 0);
    }

    private void Jump() {
        if(!_canJump) return;

        UIManager.Instance.PlayAudio(3);
        _rb.AddForce(0, _jumpForce, 0, ForceMode.Impulse);
        StartCoroutine(JumpTimer());
    }

    private IEnumerator JumpTimer() {
       _canJump = false;
       yield return new WaitForSeconds((float) .75);
       _canJump = true;
    }

    private void PlayerHit() {
        _lives--;
        _livesChangedEvent.Invoke(_lives);
        StartCoroutine(HitTimer());

        if(_lives==0) {
            PlayerDied();
            _canGetHit = false;
        }
        else if (_walkSpeed > 8.0)
            _walkSpeed-=(float)0.5;
    }

    private IEnumerator HitTimer() {
       _canGetHit = false;
       GetComponent<Animator>().enabled = true;
       yield return new WaitForSeconds((float) 2);
       GetComponent<Animator>().enabled = false;
       _canGetHit = true;
    }

    private void PlayerDied() {
        GameManager.Instance.GameOver();

        _canWalk = false;
        _playerMesh.GetComponent<Animator>().speed = 1;
        GetComponent<Animator>().Play("GameOver");
    }

    public float GetWalkSpeed() => _walkSpeed;

    public void IncreaseWalkSpeed() { 
        _walkSpeed+=(float)0.5;
        _playerMesh.GetComponent<Animator>().speed += (float)0.025;
        UIManager.Instance.PlayAudio(1);
        StartCoroutine(ParticleTimer());
    }

     private IEnumerator ParticleTimer() {
       _particleSystem.Play();
       yield return new WaitForSeconds((float) 1);
       _particleSystem.Stop();
    }
    
    private void OnTriggerEnter(Collider other) {
        if((other.CompareTag("Trap") || other.CompareTag("TrapOverlay")) && _canGetHit)
            PlayerHit();
        else if(other.CompareTag("Coin")) {
            _coinPool.ReturnToPool(other.gameObject);
            GameManager.Instance.CollectibleObtained(1);
        }
        else if(other.CompareTag("Food")) {
            _foodPool.ReturnToPool(other.gameObject);
            GameManager.Instance.CollectibleObtained(10);
        }
        else if(other.CompareTag("SceneryTrigger")) {
            GameManager.Instance.SpawnScenery();
            other.enabled = false;
        }
    }
}
