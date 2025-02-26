using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public IntVariable gameScore;
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public AudioSource bgm;

    // private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        // SetScore(score);
    }

    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        SetScore();
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        bgm.Play();
    }

    public void IncreaseScore(int increment)
    {
        // increase score by 1
        gameScore.ApplyChange(1);
        // Check if high score
        if (gameScore.Value > gameScore.previousHighestValue)
        {
            //update high score
            gameScore.previousHighestValue = gameScore.Value;
        }
        SetScore();
    }

    // public void SetScore(int score)
    public void SetScore()
    {
        // invoke score change event with current score to update HUD
        scoreChange.Invoke(gameScore.Value);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
        bgm.Stop();
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(3f);

    }
}
