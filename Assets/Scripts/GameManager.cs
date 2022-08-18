using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator _cameraAnim;
    [SerializeField] private SceneSpawner _sceneSpawner;
    [SerializeField] private BugPlayer _player;
    private int _score, _highScore, _wallet = 0;

    [System.NonSerialized] public UnityEvent<int> _scoreChangedEvent;
    [System.NonSerialized] public UnityEvent<bool> _gameOverEvent;

    public static GameManager Instance {get; private set;}

    private void Awake() { 
        Instance = this;
        Application.targetFrameRate = 60;
    }

    private void OnEnable() {
        if(PlayerPrefs.HasKey("HighScore")) 
            _highScore = PlayerPrefs.GetInt("HighScore");

        if(PlayerPrefs.HasKey("Wallet"))
            _wallet = PlayerPrefs.GetInt("Wallet");

        if(_scoreChangedEvent == null)
            _scoreChangedEvent = new UnityEvent<int>();
            
         if(_gameOverEvent == null)
            _gameOverEvent = new UnityEvent<bool>();
    }

    private void OnDisable() {
            PlayerPrefs.SetInt("HighScore", _highScore);
            PlayerPrefs.SetInt("Wallet", _wallet);
            PlayerPrefs.Save();
    }

    public void CollectibleObtained(int value) {
        _score += value;
    
        if(_score%8==0 && _player.GetWalkSpeed() < 15)
            _player.IncreaseWalkSpeed();

        if(_score%100==0 && _player.GetWalkSpeed() <= 20.0)
            _player.IncreaseWalkSpeed();

        _scoreChangedEvent.Invoke(_score);
    }

    public void SpawnScenery() {
        _sceneSpawner.SpawnScenery();
    }

    public void GameOver() {
        _gameOverEvent.Invoke(true);

        if(_score > _highScore)
            _highScore = _score;

        _wallet += _score;

        _cameraAnim.enabled = true;
        UIManager.Instance.UpdateValuesOnGameOver(_score, _highScore, _wallet);
    }

    public void ChangeScene(string scene) {
        Time.timeScale = 1f;
        StartCoroutine(TransitionTimer(scene));
    }

    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene); 
    }
}
