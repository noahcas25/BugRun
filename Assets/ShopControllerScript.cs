using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopControllerScript : MonoBehaviour
{

    [SerializeField]
    private GameObject content;

    private int bugIndex;
    private float[] bugPositions;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

    // Initializing positions to move the camera to for each bug.

        // int numBugs = content.GetChild().childCount;
        // bugPositions = new float[numBugs];

        // for(int i; i < numBugs; i++) {
        //     bugPositions[i] = -437.9 + i(81.3);
        // }

        setBugPositions();
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex"))
            bugIndex = PlayerPrefs.GetInt("bugIndex");
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
         bugIndex = index;
          Debug.Log(bugIndex);
         content.GetComponent<RectTransform>().anchoredPosition = new Vector3(bugPositions[index], 0, 0);
    }

     public void ChangeScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}
