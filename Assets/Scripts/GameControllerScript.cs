using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{

    public GameObject player;

    private int score = 0;
    private GameObject scoreText;
    private GameObject livesText;
    public GameObject particleSystem1;
    public GameObject sceneSpawner;
 

    void Start() {
        scoreText = GameObject.FindWithTag("Score");
        livesText= GameObject.FindWithTag("LivesCount");
        player = GameObject.FindWithTag("Player");

        scoreText.GetComponent<Animator>().enabled = false;
        Application.targetFrameRate = 60;
    }

    public void CollectibleObtained(int value) {
        score += value;

        if(score%8==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() < 15) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
            StartCoroutine(ParticleTimer());
            StartCoroutine(AnimatorTimer(scoreText));
        }

        // StartCoroutine(AnimatorTimer(scoreText));
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + score;
    }

    public void SpawnScenery() {
        sceneSpawner.GetComponent<SceneSpawner>().SpawnScenery();
    }

    public void PlayerHit() {
        livesText.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<PlayerControllerScript>().GetLives() + "";
        // StartCoroutine(AnimatorTimer(slowDownArrow));
    }

    public void PlayerDied() {
         Time.timeScale = 0f;
         transform.GetChild(0).gameObject.SetActive(false);
         transform.GetChild(1).gameObject.SetActive(true);
    }

    public void LevelCompleted() {
        Time.timeScale = 0f;
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void Restart() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("BugRun");
    }
    
    private IEnumerator AnimatorTimer(GameObject animator) {
       animator.GetComponent<Animator>().enabled = true;
       yield return new WaitForSeconds((float) 1.5);
       animator.GetComponent<Animator>().enabled = false;
    }

     private IEnumerator ParticleTimer() {
       particleSystem1.GetComponent<ParticleSystem>().Play();
       yield return new WaitForSeconds((float) 1);
       particleSystem1.GetComponent<ParticleSystem>().Stop();
    }

    // private IEnumerator PowerUpTimer() {
    //    player.GetComponent<PlayerControllerScript>().canGetHit = false;
    //    yield return new WaitForSeconds((float) 1);
    //    player.GetComponent<PlayerControllerScript>().canGetHit = true;
    // }
}
