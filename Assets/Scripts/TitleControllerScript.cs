using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject volumeSlider;
    
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject settingsCanvas;

     [SerializeField]
    private GameObject transitionCanvas;

    private float volume = 100;
    private bool paused = false;
    private int bugIndex = 0;

    void Start() {
        Application.targetFrameRate = 60;
        // Look into was happenin here
        audioSource = GetComponent<AudioSource>(); 
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            player.transform.GetChild(bugIndex).gameObject.SetActive(false);
            bugIndex = PlayerPrefs.GetInt("bugIndex");
            player.transform.GetChild(bugIndex).gameObject.SetActive(true);
        }
        
        if(PlayerPrefs.HasKey("Volume")) {
            volume = PlayerPrefs.GetFloat("Volume"); 
            audioSource = GetComponent<AudioSource>();  
            audioSource.volume = volume;
            volumeSlider.GetComponent<Slider>().value = volume; 
        }

        transitionCanvas.GetComponent<Animator>().CrossFade("TransitionIn", 0, 0, 0, 0);
    }

    void OnDisable() {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("bugIndex", bugIndex);
        PlayerPrefs.Save();
    }

    public void Pause() {
        if(!paused){
            settingsCanvas.SetActive(true);
            PlayerPrefs.DeleteAll();
            paused = true;
        }
        else {
            settingsCanvas.SetActive(false);
            paused = false;
        }
    }

    public void ChangeScene(string scene) {
       Time.timeScale = 1f;
       transitionCanvas.GetComponent<Animator>().CrossFade("TransitionOut", 0, 0, 0, 0);
       StartCoroutine(TransitionTimer(scene));
    }

    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene);
    }

    public void UpdateVolume() {
        volume = volumeSlider.GetComponent<Slider>().value;
        audioSource.volume = volume;
    }
}
