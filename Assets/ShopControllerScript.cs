using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopControllerScript : MonoBehaviour
{

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private AudioSource audioSource;

    private int bugIndex = 0;
    private int newIndex = 0;
    private float[] bugPositions;
    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        setBugPositions();
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex"))
            newIndex = PlayerPrefs.GetInt("bugIndex");

        if(PlayerPrefs.HasKey("Volume")) {
            volume = PlayerPrefs.GetFloat("Volume");
            audioSource = GetComponent<AudioSource>();  
            audioSource.volume = volume;
        }

        setBugPositions();
        PlayerSelect(newIndex);
    }

    void OnDisable() {
        PlayerPrefs.SetInt("bugIndex", bugIndex);
        PlayerPrefs.Save();
    }

// Initializing positions to move the camera to for each bug.
    private void setBugPositions() {
        int numBugs = content.transform.GetChild(0).childCount;
        bugPositions = new float[numBugs];

        for(int i = 0; i < numBugs; i++) {
            bugPositions[i] = (float)(437.9 - (i * 81.3));
        }
    }

// Based on the bugPositions index -> Move the scroll views content to center screen.
    public void PlayerSelect(int index) {
         content.transform.GetChild(2).GetChild(bugIndex).gameObject.SetActive(false);

         bugIndex = index;
         content.GetComponent<RectTransform>().anchoredPosition = new Vector3(bugPositions[bugIndex], 0, 0);
         content.transform.GetChild(2).GetChild(bugIndex).gameObject.SetActive(true);
    }

     public void ChangeScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}
