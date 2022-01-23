using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
// Variables
    // Game Components
    [SerializeField]
    private GameObject player, camera, sceneSpawner, particleSystem1;

    // Ui elements
    [SerializeField]
    private GameObject scoreText, livesText, restartCanvasScore, restartCanvasHS, restartCanvasWallet, volumeSlider, transitionCanvas;

    // AudioTracks
    [SerializeField]
    private AudioClip gameOver, coinCollect, playerHit;
    private AudioSource audioSource;
    private float volume = 100;
    private int bugIndex = 0;
    private int score = 0;
    private int highScore;
    private int wallet = 0;
    private bool gamesOver = false;
 
 // Start is called before the first frame update - sets target framerate
    void Start() {
        Application.targetFrameRate = 60;
    }

// Function called OnEnable of the scene, gathers playerprefs
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
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.SetInt("Wallet", wallet);
            PlayerPrefs.SetFloat("Volume", volume);
            PlayerPrefs.Save();
    }

// Function called when collectibles are obtained - plays audio, increases walkspeed, changes UI score
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

// Called when Players hit - updates Ui element for lives, plays audio
    public void PlayerHit() {
        livesText.GetComponent<Text>().text = player.GetComponent<PlayerControllerScript>().GetLives() + "";
        audioSource.PlayOneShot(playerHit);
    }

// Called when Player Dies - plays audio, stops player walking, plays animations
    public void PlayerDied() {
         audioSource.PlayOneShot(gameOver);
         player.GetComponent<PlayerControllerScript>().setCanWalk(false);
         player.GetComponent<PlayerControllerScript>().GetPlayerMesh().GetComponent<Animator>().speed = 1;
         player.GetComponent<Animator>().Play("GameOver");
         camera.GetComponent<Animator>().enabled = true;
         gamesOver = true;

         StartCoroutine(DieTimer());
    }

// Calls on the SceneSpawner to spawn in the next scenery.
    public void SpawnScenery() {
        sceneSpawner.GetComponent<SceneSpawner>().SpawnScenery();
    }

// Pauses the game, stops time and opens settings canvas
    public void Pause() {
        if(!gamesOver) {
            Time.timeScale = 0f;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }

// Resumes the game, reinstates timescale and closes settings canvas
    public void Resume() {
        Time.timeScale = 1f;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(false);
    }

// Timer that plays the speed up particle system
     private IEnumerator ParticleTimer() {
       particleSystem1.GetComponent<ParticleSystem>().Play();
       yield return new WaitForSeconds((float) 1);
       particleSystem1.GetComponent<ParticleSystem>().Stop();
    }

// Timer and functions that open the gameOverCanvas, updates UI elements, updates highscore and wallet values
    private IEnumerator DieTimer() {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds((float)2.25);
        transform.GetChild(2).gameObject.SetActive(true);

        if(score > highScore)
            highScore = score;

        wallet += score;
        restartCanvasWallet.GetComponent<Text>().text = "" + wallet;
        restartCanvasScore.GetComponent<Text>().text = scoreText.GetComponent<Text>().text;
        restartCanvasHS.GetComponent<Text>().text = "" + highScore;
        restartCanvasWallet.GetComponent<Text>().text = "" + wallet;
        Time.timeScale = 0f;
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
