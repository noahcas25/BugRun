using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopControllerScript : MonoBehaviour
{

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject unlockButtons;

    [SerializeField]
    private GameObject walletText;

    [SerializeField]
    private GameObject transitionCanvas;

    private int bugIndex = 0;
    private int newIndex = 0;
    private float[] bugPositions;
    private bool[] unlocked;
    private float volume;
    private int wallet;

    // For purchasing bugs
    private int purchaseIndex;
    private int cost;
    private AdsManager ads;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        ads = new AdsManager();
    }

    void OnEnable() {
        SetBugPositions();

        if(PlayerPrefs.HasKey("bugIndex"))
            newIndex = PlayerPrefs.GetInt("bugIndex");

        if(PlayerPrefs.HasKey("Volume")) {
            volume = PlayerPrefs.GetFloat("Volume");
            audioSource = GetComponent<AudioSource>();  
            audioSource.volume = volume;
        }

        if(PlayerPrefs.HasKey("Wallet")) {
            wallet = PlayerPrefs.GetInt("Wallet");
            walletText.GetComponent<Text>().text = "" + wallet;
        }

        for(int i = 1; i < unlocked.Length; i++) {
            if(PlayerPrefs.HasKey("00" + i)) {
                unlocked[i] = true;
            }
            else{
                unlocked[i] = false;
                unlockButtons.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        PlayerSelect(newIndex);
        transitionCanvas.GetComponent<Animator>().CrossFade("TransitionIn", 0, 0, 0, 0);
    }


    void OnDisable() {
        PlayerPrefs.SetInt("bugIndex", bugIndex);
        PlayerPrefs.SetInt("Wallet", wallet);

        for(int i = 0; i < unlocked.Length; i++) {
            if(unlocked[i] == true) 
                PlayerPrefs.SetInt("00" + i, 1);
        }


        PlayerPrefs.Save();
    }

// Initializing positions to move the camera to for each bug.
    private void SetBugPositions() {
        int numBugs = content.transform.GetChild(0).childCount;
        bugPositions = new float[numBugs];
        unlocked = new bool[numBugs];
        unlocked[0] = true;

        for(int i = 0; i < numBugs; i++) {
            bugPositions[i] = (float)(437.9 - (i * 81.3));
        }
    }

 // Methods used for purchasing

    public void UnlockCharacter(string values) {
        string[] splitValues = values.Split(';');
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        purchaseIndex = int.Parse(splitValues[0]);
        this.cost = int.Parse(splitValues[1]);
    }

    public void ConfirmPurchase(bool answer) {
        if(answer==true){
            if(unlocked[purchaseIndex] == false && wallet >= cost) {
                unlocked[purchaseIndex] = true;
                wallet -= cost;
                UpdateInformation(true); 
            } else if(wallet < cost) {
                StartCoroutine(InsufficientTimer());
            }
        }

            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator InsufficientTimer() {
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        transform.GetChild(2).gameObject.SetActive(false);
    }
    

    private void UpdateInformation(bool purchasing) {
        if(purchasing)
            unlockButtons.transform.GetChild(purchaseIndex).gameObject.SetActive(false);
        
        walletText.GetComponent<Text>().text = "" + wallet;
    }

// Based on the bugPositions index -> Move the scroll views content to center screen.
    public void PlayerSelect(int index) {
        if(unlocked[index] == true) {
            content.transform.GetChild(2).GetChild(bugIndex).gameObject.SetActive(false);

            bugIndex = index;
            content.GetComponent<RectTransform>().anchoredPosition = new Vector3(bugPositions[bugIndex], 0, 0);
            content.transform.GetChild(2).GetChild(bugIndex).gameObject.SetActive(true);
        }
    }

     public void ChangeScene(string scene) {
        transitionCanvas.GetComponent<Animator>().CrossFade("TransitionOut", 0, 0, 0, 0);
        StartCoroutine(TransitionTimer(scene));
    }

    public void PlayAd() {
        ads.PlayAd();
        wallet += 500;
        UpdateInformation(false);
    }

    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene);
    }
}
