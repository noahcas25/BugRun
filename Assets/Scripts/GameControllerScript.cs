using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private GameObject livesText;
    [SerializeField]
    private GameObject particleSystem1;
    [SerializeField]
    private GameObject sceneSpawner;

    private int bugIndex = 0;
    private int score = 0;
    private int lifeTimeScore;
 

    void Start() {
        Application.targetFrameRate = 60;
    }

    void OnEnable() {
        if(PlayerPrefs.HasKey("bugIndex")) {
            player.transform.GetChild(bugIndex).gameObject.SetActive(false);
            bugIndex = PlayerPrefs.GetInt("bugIndex");
            player.transform.GetChild(bugIndex).gameObject.SetActive(true);
        }

        if(PlayerPrefs.HasKey("lifeTimeScore")) {
            lifeTimeScore = PlayerPrefs.GetInt("lifeTimeScore");
    }
}
    // void OnDisable() {
        
    // }

    public void CollectibleObtained(int value) {
        score += value;

        if(score%8==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() < 15) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
            StartCoroutine(ParticleTimer());
            // StartCoroutine(AnimatorTimer(scoreText));
        }

        scoreText.GetComponent<Text>().text = "" + score;
    }

    public void SpawnScenery() {
        sceneSpawner.GetComponent<SceneSpawner>().SpawnScenery();
    }

    public void PlayerHit() {
        livesText.GetComponent<Text>().text = player.GetComponent<PlayerControllerScript>().GetLives() + "";
    }

    public void PlayerDied() {
         StartCoroutine(DieTimer());
    }

    // public void LevelCompleted() {
    //     Time.timeScale = 0f;
    //     transform.GetChild(2).gameObject.SetActive(true);
    // }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("BugRun");
    }

    public void Pause() {
        Time.timeScale = 0f;
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1f;
        transform.GetChild(2).gameObject.SetActive(false);
    }
    
    // private IEnumerator AnimatorTimer(GameObject animator) {
    //    animator.GetComponent<Animator>().enabled = true;
    //    yield return new WaitForSeconds((float) 1.5);
    //    animator.GetComponent<Animator>().enabled = false;
    // }

     private IEnumerator ParticleTimer() {
       particleSystem1.GetComponent<ParticleSystem>().Play();
       yield return new WaitForSeconds((float) 1);
       particleSystem1.GetComponent<ParticleSystem>().Stop();
    }

    private IEnumerator DieTimer() {
        player.GetComponent<PlayerControllerScript>().setCanWalk(false);
        transform.GetChild(0).gameObject.SetActive(false);
        player.GetComponent<Animator>().Play("GameOver");
        camera.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds((float)2.25);
        transform.GetChild(1).gameObject.SetActive(true);
         Time.timeScale = 0f;
        
    }

    public void ChangeScene(string scene) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
}
