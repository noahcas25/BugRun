using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText, _livesText, _restartCanvasScoreText, _restartCanvasHSText, _restartCanvasWalletText;
    [SerializeField] private GameObject _gameUICanvas, _gameOverCanvas, _optionsCanvas;
    [SerializeField] private Animator _transitionAnim;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private AudioClip[] _audioClips;

    private float _volume = 100;
    private AudioSource _audioSource;

    public static UIManager Instance {get; private set;}

    private void Awake() {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        if(PlayerPrefs.HasKey("Volume")) {
            _volume = PlayerPrefs.GetFloat("Volume");
            _audioSource.volume = _volume;
            _volumeSlider.value = _volume;
        }

        GameManager.Instance._scoreChangedEvent.AddListener(UpdateScoreText);
        BugPlayer.Instance._livesChangedEvent.AddListener(UpdateLives);
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat("Volume", _volume);
        PlayerPrefs.Save();
    }

    private void Start() => _transitionAnim.CrossFade("TransitionIn", 0, 0, 0, 0);

    private void UpdateLives(int lives) {
        _livesText.text = lives + "";
        PlayAudio(2);
    }

    private void UpdateScoreText(int score) {
        _scoreText.text = score + "";
        PlayAudio(0);
    } 

    public void UpdateValuesOnGameOver(int score, int highScore, int wallet) {
        _restartCanvasScoreText.text = "" + score;
        _restartCanvasHSText.text = "" + highScore;
        _restartCanvasWalletText.text = "" + wallet;

        PlayAudio(4);
        StartCoroutine(DieTimer());
    }

    private IEnumerator DieTimer(){
        yield return new WaitForSeconds((float)2.25);
        _gameUICanvas.SetActive(false);
        _gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Pause() {
        Time.timeScale = 0f;
        _gameUICanvas.SetActive(false);
        _optionsCanvas.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1f;
        _gameUICanvas.SetActive(true);
        _optionsCanvas.SetActive(false);
    }

    public void PlayAudio(int index) {
        _audioSource.PlayOneShot(_audioClips[index]);
    }

    public void UpdateVolume() {
        _volume = _volumeSlider.value;
        _audioSource.volume = _volume;
    }

    public void ChangeScene(string scene) {
        _transitionAnim.CrossFade("TransitionOut", 0, 0, 0, 0);
        GameManager.Instance.ChangeScene(scene);
    }
}
