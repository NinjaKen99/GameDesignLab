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
    private Rigidbody2D marioBody;
    public Animator marioAnimator;
    public AudioSource marioAudio; // for audio

    // Non-player variables
    public TextMeshProUGUI scoreText;
    public Vector3 textStartPosition = new Vector3(-695.0f, 492.0f, 0.0f);
    public RectTransform buttonRect;
    public Vector3 buttonStartPosition = new Vector3(895.0f, 485.0f, 0.0f);
    public GameObject gameOverPanel;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;

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

        // Test Score text positioning
        scoreText.transform.localPosition = textStartPosition;
        scoreText.alignment = TextAlignmentOptions.MidlineLeft;
        buttonRect.localPosition = buttonStartPosition;
        gameOverPanel.SetActive(false);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
            Time.timeScale = 0.0f;
            gameOverPanel.SetActive(true);
            scoreText.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            scoreText.alignment = TextAlignmentOptions.Center;
            buttonRect.localPosition = new Vector3(0.0f, -90.0f, 0.0f);
        }
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.41f, -0.48f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = textStartPosition;
        scoreText.alignment = TextAlignmentOptions.MidlineLeft;
        buttonRect.localPosition = buttonStartPosition;
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<GoombaMovement>().startPosition;
        }
        // reset score
        jumpOverGoomba.score = 0;
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
        if (Input.GetKeyDown("d") && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.1f)
                marioAnimator.SetTrigger("onSkid");
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = marioBody.velocity.y;

        if (Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // stop
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            marioBody.velocity = new Vector2(0.0f, moveVertical);
        }

        // jump
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
}
