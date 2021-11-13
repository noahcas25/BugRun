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

    void Start() {
        scoreText = GameObject.FindWithTag("Score");
        livesText= GameObject.FindWithTag("LivesCount");
        player = GameObject.FindWithTag("Player");

        scoreText.GetComponent<Animator>().enabled = false;
        Application.targetFrameRate = 60;
    }

    public void FoodObtained() {
        score++;

        if(score%10==0 && player.GetComponent<PlayerControllerScript>().GetWalkSpeed() < 14) {
            player.GetComponent<PlayerControllerScript>().IncreaseWalkSpeed();
        }

        StartCoroutine(AnimatorTimer());
        scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = "" + score;
    }

    public void PlayerHit() {
        livesText.GetComponent<TMPro.TextMeshProUGUI>().text = player.GetComponent<PlayerControllerScript>().GetLives() + "";
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
    
    private IEnumerator AnimatorTimer() {
       scoreText.GetComponent<Animator>().enabled = true;
       yield return new WaitForSeconds((float) 1.5);
       scoreText.GetComponent<Animator>().enabled = false;
    }
}
