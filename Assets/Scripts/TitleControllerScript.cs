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

    private float volume;
    private bool paused = false;
    private int bugIndex = 0;

    void Start() {
        Application.targetFrameRate = 60;
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
    }

    void OnDisable() {
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

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

    public void ChangeScene(string scene) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }

    public void UpdateVolume() {
        volume = volumeSlider.GetComponent<Slider>().value;
        audioSource.volume = volume;
    }
}
