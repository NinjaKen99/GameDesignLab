using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public IntVariable gameScore;
    // events
    public UnityEvent gameStart;
    // public UnityEvent gameRestart;
    public UnityEvent updateScore;
    // public UnityEvent gameOver;
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
    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        updateScore.Invoke();
    }
    public void IncreaseScore(int increment)
    {
        // increase score by 1
        gameScore.ApplyChange(increment);
        // Check if high score
        if (gameScore.Value > gameScore.previousHighestValue)
        {
            //update high score
            gameScore.previousHighestValue = gameScore.Value;
        }
        updateScore.Invoke();
    }
    public void PauseGame()
    {
        Time.timeScale = 0.0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
    public void GameOver()
    {
        Time.timeScale = 0.0f;
        // gameOver.Invoke();
        bgm.Stop();
    }
    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        updateScore.Invoke();
        // SetScore();
        // gameRestart.Invoke();
        Time.timeScale = 1.0f;
        bgm.Play();
    }
    public void RequestPowerupEffect(IPowerup i)
    {
        switch (i.powerupType)
        {
            case PowerupType.Coin:
                IncreaseScore(1);
                break;
            case PowerupType.OneUpMushroom:
                Debug.Log("Increase lives");
                break;
        }
    }
    // public void SetScore()
    // {
    //     // invoke score change event with current score to update HUD
    //     scoreChange.Invoke(gameScore.Value);
    // }
}
