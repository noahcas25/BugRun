using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ShopController : MonoBehaviour
{
    [SerializeField] private GameObject _content, _confirmPurchaseCanvas, _insufficientFundsCanvas, _unlockButtons;
    [SerializeField] private Animator _transitionCanvas;
    [SerializeField] private Text _walletText;
    [SerializeField] private Transform _selectedIndicators;
    
    private AudioSource _audioSource;
    private int _bugIndex, _newIndex, _wallet;
    private float[] _bugPositions;
    private bool[] _unlocked;

    // For purchasing bugs
    private int _purchaseIndex, _cost;

    public static ShopController Instance {get; private set;}

    private void Awake() {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        SetBugPositions();

        if(PlayerPrefs.HasKey("bugIndex"))
            _newIndex = PlayerPrefs.GetInt("bugIndex");

        if(PlayerPrefs.HasKey("Volume"))
            _audioSource.volume = PlayerPrefs.GetFloat("Volume");

        if(PlayerPrefs.HasKey("Wallet")) {
            _wallet = PlayerPrefs.GetInt("Wallet");
            _walletText.text = _wallet + "";
        }

    // If the playerPref (key) "00X" exist then the skins been unlocked
        for(int i = 1; i < _unlocked.Length; i++) {
            if(PlayerPrefs.HasKey("00" + i))
                _unlocked[i] = true;
            else _unlockButtons.transform.GetChild(i).gameObject.SetActive(true);
        }

        PlayerSelect(_newIndex);
        _transitionCanvas.CrossFade("TransitionIn", 0, 0, 0, 0);
    }

// Functions called OnDisable of the scene, saves playerprefs
    private void OnDisable() {
        PlayerPrefs.SetInt("bugIndex", _bugIndex);
        PlayerPrefs.SetInt("Wallet", _wallet);

        for(int i = 0; i < _unlocked.Length; i++) {
            if(_unlocked[i] == true) 
                PlayerPrefs.SetInt("00" + i, 1);
        }

        PlayerPrefs.Save();
    }

    private void Start() {
        Application.targetFrameRate = 60;
        AdsManager.Instance.LoadAd("Rewarded_");
    } 

// Initializing _bugPositions to move the camera to for each bug.
    private void SetBugPositions() {
        int numBugs = _content.transform.GetChild(0).childCount;
        _bugPositions = new float[numBugs];
        _unlocked = new bool[numBugs];
        _unlocked[0] = true;

        for(int i = 0; i < numBugs; i++) {
            _bugPositions[i] = (float)(437.9 - (i * 81.3));
        }
    }

// Methods used for purchasing
    // splits string between purchase index and cost
    public void UnlockCharacter(string values) {
        _confirmPurchaseCanvas.SetActive(true);

        string[] splitValues = values.Split(';');
        _purchaseIndex = int.Parse(splitValues[0]);
        this._cost = int.Parse(splitValues[1]);
    }

    // subtracts from wallet if purchase is confirmed and allows bug to be selected
    public void ConfirmPurchase(bool proceed) {
        _confirmPurchaseCanvas.SetActive(false);

        if(!proceed) return;

        if(!_unlocked[_purchaseIndex] && _wallet >= _cost) {
            _unlocked[_purchaseIndex] = true;
            _wallet -= _cost;
            UpdateInformation(true); 
        } else if(_wallet < _cost)
            StartCoroutine(InsufficientTimer());
    }

    private IEnumerator InsufficientTimer() {
        _insufficientFundsCanvas.SetActive(true);
        yield return new WaitForSeconds(3);
        _insufficientFundsCanvas.SetActive(false);
    }
    
    // Updates store information about bugs and wallet amount
    private void UpdateInformation(bool purchasing) {
        if(purchasing)
            _unlockButtons.transform.GetChild(_purchaseIndex).gameObject.SetActive(false);
        
        _walletText.text = "" + _wallet;
    }

// Based on the bugPositions index -> Move the scroll views _content to center screen.
    public void PlayerSelect(int index) {
        if(!_unlocked[index]) return;
        
        _selectedIndicators.GetChild(_bugIndex).gameObject.SetActive(false);

        _bugIndex = index;
        _content.GetComponent<RectTransform>().anchoredPosition = new Vector3(_bugPositions[_bugIndex], 0, 0);
        _selectedIndicators.GetChild(_bugIndex).gameObject.SetActive(true);
    }
    
     public void ChangeScene(string scene) {
        _transitionCanvas.CrossFade("TransitionOut", 0, 0, 0, 0);
        StartCoroutine(TransitionTimer(scene));
    }

    private IEnumerator TransitionTimer(string scene) {
        yield return new WaitForSeconds((float) 0.5);
        SceneManager.LoadScene(scene);
    }

    public void PlayAd()  {
         AdsManager.Instance.PlayRewardAd();
    }

    public void GiveReward() {
        _wallet += 500;
        UpdateInformation(false);
        AdsManager.Instance.LoadAd("Rewarded_");
    }
}
