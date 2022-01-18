using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    // Game Components
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject sceneSpawner;
    [SerializeField]
    private GameObject particleSystem1;

    // Ui elements
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private GameObject livesText;
    [SerializeField]
    private GameObject restartCanvasScore;
    [SerializeField]
    private GameObject restartCanvasHS;
    [SerializeField]
    private GameObject restartCanvasWallet;
    [SerializeField]
    private GameObject volumeSlider;
     [SerializeField]
    private GameObject transitionCanvas;

    // AudioTracks
    [SerializeField]
    private AudioClip gameOver;
    [SerializeField]
    private AudioClip coinCollect;
    [SerializeField]
    private AudioClip playerHit;


    private AudioSource audioSource;
    private float volume = 100;
    private int bugIndex = 0;
    private int score = 0;
    private int highScore;
    private int wallet = 0;
    private bool gamesOver = false;
 

    void Start() {
        Application.targetFrameRate = 60;
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            player.transform.GetChild(bugIndex).gameObject.SetActive(false);
            bugIndex = PlayerPrefs.GetInt("bugIndex");
            player.transform.GetChild(bugIndex).gameObject.SetActive(true);
            player.GetComponent<PlayerControllerScript>().SetPlayerMesh(player.transform.GetChild(bugIndex).gameObject);
        } else player.GetComponent<PlayerControllerScript>().SetPlayerMesh(GameObject.FindWithTag("PlayerMesh"));

        if(PlayerPrefs.HasKey("HighScore")) 
            highScore = PlayerPrefs.GetInt("HighScore");

        if(PlayerPrefs.HasKey("Wallet"))
            wallet = PlayerPrefs.GetInt("Wallet");

        
        if(PlayerPrefs.HasKey("Volume")) {
            volume = PlayerPrefs.GetFloat("Volume"); 
            audioSource = GetComponent<AudioSource>();  
            audioSource.volume = volume;
            volumeSlider.GetComponent<Slider>().value = volume; 
        }

        transitionCanvas.GetComponent<Animator>().CrossFade("TransitionIn", 0, 0, 0, 0);
}
    void OnDisable() {
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.SetInt("Wallet", wallet);
            PlayerPrefs.SetFloat("Volume", volume);
            PlayerPrefs.Save();
    }

    public void UpdateVolume() {
        volume = volumeSlider.GetComponent<Slider>().value;
        audioSource.volume = volume;
    }

    public void CollectibleObtained(int value) {
        score += value;
        audioSource.PlayOneShot(coinCollect);

        if(score%8==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() < 15) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
            player.GetComponent<PlayerControllerScript>().GetPlayerMesh().GetComponent<Animator>().speed += (float)0.025;
            StartCoroutine(ParticleTimer());
        }

        if(score%100==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() <= 20.0) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
            StartCoroutine(ParticleTimer());
        }

        scoreText.GetComponent<Text>().text = "" + score;
    }

    public void SpawnScenery() {
        sceneSpawner.GetComponent<SceneSpawner>().SpawnScenery();
    }

    public void PlayerHit() {
        livesText.GetComponent<Text>().text = player.GetComponent<PlayerControllerScript>().GetLives() + "";
        audioSource.PlayOneShot(playerHit);
    }

    public void PlayerDied() {
         audioSource.PlayOneShot(gameOver);
         player.GetComponent<PlayerControllerScript>().setCanWalk(false);
         player.GetComponent<PlayerControllerScript>().GetPlayerMesh().GetComponent<Animator>().speed = 1;
         player.GetComponent<Animator>().Play("GameOver");
         camera.GetComponent<Animator>().enabled = true;
         gamesOver = true;

         StartCoroutine(DieTimer());
    }

    public void Pause() {
        if(!gamesOver) {
            Time.timeScale = 0f;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    public void Resume() {
        Time.timeScale = 1f;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }

     private IEnumerator ParticleTimer() {
       particleSystem1.GetComponent<ParticleSystem>().Play();
       yield return new WaitForSeconds((float) 1);
       particleSystem1.GetComponent<ParticleSystem>().Stop();
    }

    private IEnumerator DieTimer() {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds((float)2.25);
        transform.GetChild(2).gameObject.SetActive(true);

    // Updates Ui Elements
        if(score > highScore)
            highScore = score;

        wallet += score;
        restartCanvasWallet.GetComponent<Text>().text = "" + wallet;
        restartCanvasScore.GetComponent<Text>().text = scoreText.GetComponent<Text>().text;
        restartCanvasHS.GetComponent<Text>().text = "" + highScore;
        restartCanvasWallet.GetComponent<Text>().text = "" + wallet;
        Time.timeScale = 0f;
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
}
