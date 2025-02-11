using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-695, 492, 0),  //Initial Position
        new Vector3(0, 0, 0)        //Gameover Position
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(895, 485, 0),   // Initial Position
        new Vector3(0, -90, 0)      // Gameover Position
    };

    public TextMeshProUGUI scoreText;
    public Transform restartButton;
    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        scoreText.alignment = TextAlignmentOptions.MidlineLeft;
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        scoreText.alignment = TextAlignmentOptions.Center;
        restartButton.localPosition = restartButtonPosition[1];
    }
}
