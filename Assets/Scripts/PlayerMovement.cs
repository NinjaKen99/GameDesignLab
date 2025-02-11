using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxSpeed = 20;
    public float upSpeed = 10;
    private bool onGroundState = true;
    private bool moving = false;
    private bool jumpedState = false;
    private Rigidbody2D marioBody;
    public Animator marioAnimator;
    public AudioSource marioAudio; // for audio
    public AudioClip marioDeath;
    public float deathImpulse = 10;

    [System.NonSerialized]
    public bool alive = true;

    // Non-player variables
    // public TextMeshProUGUI scoreText;
    // public Vector3 textStartPosition = new Vector3(-695.0f, 492.0f, 0.0f);
    // public RectTransform buttonRect;
    // public Vector3 buttonStartPosition = new Vector3(895.0f, 485.0f, 0.0f);
    // public GameObject gameOverPanel;
    public Transform gameCamera;
    public GameObject enemies;
    // public JumpOverGoomba jumpOverGoomba;

    // global variables
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

        // // Test Score text positioning
        // scoreText.transform.localPosition = textStartPosition;
        // scoreText.alignment = TextAlignmentOptions.MidlineLeft;
        // buttonRect.localPosition = buttonStartPosition;
        // gameOverPanel.SetActive(false);
    }
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
    // void GameOverScene()
    // {
    //     // stop time
    //     Time.timeScale = 0.0f;
    //     // set gameover scene
    //     gameOverPanel.SetActive(true);
    //     scoreText.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
    //     scoreText.alignment = TextAlignmentOptions.Center;
    //     buttonRect.localPosition = new Vector3(0.0f, -90.0f, 0.0f);
    // }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
            // play death animation
            marioAnimator.Play("mario_die");
            marioAudio.PlayOneShot(marioDeath);
            alive = false;
        }
    }

    // public void RestartButtonCallback(int input)
    // {
    //     Debug.Log("Restart!");
    //     // reset everything
    //     ResetGame();
    //     // resume time
    //     Time.timeScale = 1.0f;
    // }

    // private void ResetGame()
    // {
    //     // reset position
    //     marioBody.transform.position = new Vector3(-3.41f, -0.48f, 0.0f);
    //     // marioBody.GetComponent<Collider2D>().enabled = true;
    //     // reset sprite direction
    //     faceRightState = true;
    //     marioSprite.flipX = false;
    //     // // reset score
    //     // scoreText.text = "Score: 0";
    //     // gameOverPanel.SetActive(false);
    //     // scoreText.transform.localPosition = textStartPosition;
    //     // scoreText.alignment = TextAlignmentOptions.MidlineLeft;
    //     // buttonRect.localPosition = buttonStartPosition;
    //     // // reset Goomba
    //     // foreach (Transform eachChild in enemies.transform)
    //     // {
    //     //     eachChild.transform.localPosition = eachChild.GetComponent<GoombaMovement>().startPosition;
    //     // }
    //     // // reset score
    //     // jumpOverGoomba.score = 0;
    //     // reset animation
    //     marioAnimator.SetTrigger("gameRestart");
    //     alive = true;
    //     // reset camera position
    //     gameCamera.position = new Vector3(1.82f, 2.03f, -10.0f);
    // }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.41f, -0.48f, 0.0f);
        marioBody.GetComponent<Collider2D>().enabled = true;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(1.82f, 2.03f, -10.0f);
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.GetComponent<Collider2D>().enabled = false;
        marioBody.velocity = new Vector2(0.0f, 0.0f);
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }
    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }
    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }
    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }
    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }
    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }
}
