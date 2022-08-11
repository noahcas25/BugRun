using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private GameObject _bugMesh, _settingsCanvas;
    [SerializeField] private Animator _transitionAnim;
    [SerializeField] private Slider _volumeSlider;

    private float _volume = 100;
    private int _bugIndex = 0;
    private AudioSource _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            _bugMesh.transform.GetChild(_bugIndex).gameObject.SetActive(false);
            _bugIndex = PlayerPrefs.GetInt("bugIndex");
            _bugMesh.transform.GetChild(_bugIndex).gameObject.SetActive(true);
        }

        if(PlayerPrefs.HasKey("Volume")) {
            _volume = PlayerPrefs.GetFloat("Volume"); 
            _audioSource.volume = _volume;
            _volumeSlider.value = _volume; 
        }

        _transitionAnim.CrossFade("TransitionIn", 0, 0, 0, 0);
    }

    private void OnDisable() {
        PlayerPrefs.SetFloat("Volume", _volume);
        PlayerPrefs.Save();
    }

    private void Start() => Application.targetFrameRate = 60;
    
    public void Pause() => _settingsCanvas.SetActive(true);

    public void Resume() => _settingsCanvas.SetActive(false);

    public void ChangeScene(string scene) {
       Time.timeScale = 1f;
       _transitionAnim.CrossFade("TransitionOut", 0, 0, 0, 0);
       StartCoroutine(TransitionTimer(scene));
    }

    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene);
    }

    public void UpdateVolume() {
        _volume = _volumeSlider.value;
        _audioSource.volume = _volume;
    }
}
