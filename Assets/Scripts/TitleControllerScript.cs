using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleControllerScript : MonoBehaviour
{
// Variables
    [SerializeField]
    private GameObject player, volumeSlider, settingsCanvas, transitionCanvas;

    private float volume = 100;
    private bool paused = false;
    private int bugIndex = 0;
    private AudioSource audioSource;

// Start is called before the first frame update
    void Start() {
        Application.targetFrameRate = 60;
    }

// Function called OnEnable of the scene, gathers playerprefs
    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            player.transform.GetChild(bugIndex).gameObject.SetActive(false);
            bugIndex = PlayerPrefs.GetInt("bugIndex");
            player.transform.GetChild(bugIndex).gameObject.SetActive(true);
        }
        
        audioSource = GetComponent<AudioSource>(); 
        if(PlayerPrefs.HasKey("Volume")) {
            volume = PlayerPrefs.GetFloat("Volume"); 
            audioSource.volume = volume;
            volumeSlider.GetComponent<Slider>().value = volume; 
        }

        transitionCanvas.GetComponent<Animator>().CrossFade("TransitionIn", 0, 0, 0, 0);
    }

// Functions called OnDisable of the scene, saves playerprefs
    void OnDisable() {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetInt("bugIndex", bugIndex);
        PlayerPrefs.Save();
    }

// Function opens settings canvas in scene or closes it 
    public void Pause() {
        if(!paused){
            settingsCanvas.SetActive(true);
            paused = true;
        }
        else {
            settingsCanvas.SetActive(false);
            paused = false;
        }
    }

// Function that loads new scene
    public void ChangeScene(string scene) {
       Time.timeScale = 1f;
       transitionCanvas.GetComponent<Animator>().CrossFade("TransitionOut", 0, 0, 0, 0);
       StartCoroutine(TransitionTimer(scene));
    }

// Timer to change scene 
    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene);
    }

// Volume Slider updates the AudioSource volume
    public void UpdateVolume() {
        volume = volumeSlider.GetComponent<Slider>().value;
        audioSource.volume = volume;
    }
}
