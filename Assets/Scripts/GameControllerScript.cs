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
    // public GameObject speedUpArrow;
    // public GameObject slowDownArrow;
    public GameObject particleSystem1;
    // public GameObject particleSystem2;
    // public GameObject particleSystem3;


    void Start() {
        scoreText = GameObject.FindWithTag("Score");
        livesText= GameObject.FindWithTag("LivesCount");
        player = GameObject.FindWithTag("Player");
        // speedUpArrow.GetComponent<Animator>().enabled = false;
        // slowDownArrow.GetComponent<Animator>().enabled = false;

        scoreText.GetComponent<Animator>().enabled = false;
        Application.targetFrameRate = 60;
    }

    public void FoodObtained() {
        score++;

        if(score%8==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() < 15) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
            // StartCoroutine(AnimatorTimer(speedUpArrow));
            StartCoroutine(ParticleTimer());
        }

        StartCoroutine(AnimatorTimer(scoreText));
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + score;
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
    //    particleSystem2.GetComponent<ParticleSystem>().Play();
    //    particleSystem3.GetComponent<ParticleSystem>().Play();
       yield return new WaitForSeconds((float) 1);
       particleSystem1.GetComponent<ParticleSystem>().Stop();
    //    particleSystem2.GetComponent<ParticleSystem>().Stop();
    //    particleSystem3.GetComponent<ParticleSystem>().Stop();
    }
}
