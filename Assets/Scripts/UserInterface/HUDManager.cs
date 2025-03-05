using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-695, 492, 0),  //Initial Position
        new Vector3(0, 0, 0)        //Gameover Position
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(895, 485, 0),   // Initial Position
        new Vector3(0, -180, 0)      // Gameover Position
    };

    public TextMeshProUGUI scoreText;
    public Transform restartButton;
    public GameObject gameOverPanel;
    public GameObject highscoreText; // Need to add
    public GameObject menuButton;
    public IntVariable gameScore;

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        highscoreText.SetActive(false);
        menuButton.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        scoreText.alignment = TextAlignmentOptions.MidlineLeft;
        restartButton.localPosition = restartButtonPosition[0];
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        menuButton.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        scoreText.alignment = TextAlignmentOptions.Center;
        restartButton.localPosition = restartButtonPosition[1];
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
    public void SetScore()
    {
        Debug.Log("Changing Score text");
        scoreText.text = "Score: " + gameScore.Value.ToString();
    }



    public void ReturnToMain()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
