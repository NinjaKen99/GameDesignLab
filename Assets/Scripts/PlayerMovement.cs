using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    public UnityEvent<GameObject> goombaStomp;
    public UnityEvent gameOver;
    public UnityEvent<int> increaseScore;

    public GameConstants gameConstants;
    public BoolVariable faceRightState;
    float deathImpulse;
    float upSpeed;
    float maxSpeed;
    float speed;
    private bool onGroundState = true;
    private bool moving = false;
    private bool jumpedState = false;
    private Rigidbody2D marioBody;
    public Animator marioAnimator;
    public AudioSource marioAudio; // for audio
    public AudioSource marioDeathAudio;
    // public GameManager gameManager;

    [System.NonSerialized]
    public bool alive = true;

    public Transform gameCamera;
    public GameObject enemies;

    // global variables
    private SpriteRenderer marioSprite;


    // Start is called before the first frame update
    void Start()
    {
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;

        marioSprite = GetComponent<SpriteRenderer>();
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);

        faceRightState.SetValue(true);
    }

    // void Awake()
    // {
    //     // subscribe to Game Restart event
    //     GameManager.instance.gameRestart.AddListener(GameRestart);
    // }

    public void SetStartingPosition(Scene current, Scene next)
    {
        if (next.name == "World 1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-6.84f, 7.89f, 0.0f);
        }
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7) | (1 << 11);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }
    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        gameOver.Invoke();
    }
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Enemy"))
    //     {
    //         if (other.GetType() == typeof(BoxCollider2D))
    //         {
    //             Debug.Log("Collided with goomba!");
    //             // play death animation
    //             marioAnimator.Play("mario_die");
    //             marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
    //             alive = false;
    //         }
    //         else if (other.GetType() == typeof(PolygonCollider2D) & alive)
    //         {
    //             Debug.Log("Stomp on Goomba");
    //             goombaStomp.Invoke(other.gameObject);
    //             Vector2 bounce = new Vector2(0, Math.Abs(marioBody.velocity.y) + 5);
    //             marioBody.AddForce(bounce, ForceMode2D.Impulse);
    //             increaseScore.Invoke(1);
    //         }
    //     }
    // }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-3.41f, -0.48f, 0.0f);
        marioBody.GetComponent<Collider2D>().enabled = true;
        // reset sprite direction
        faceRightState.SetValue(true);
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
    public void RequestPowerupEffect(IPowerup i)
    {
        switch (i.powerupType)
        {
            case PowerupType.Damage:
                DamageMario();
                break;
            default:
                i.ApplyPowerup(this);
                break;
        }

    }

    public void DamageMario()
    {
        GetComponent<MarioStateController>().SetPowerup(PowerupType.Damage);
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState.Value)
        {
            faceRightState.Toggle();
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState.Value)
        {
            faceRightState.Toggle();
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
            Move(faceRightState.Value == true ? 1 : -1);
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
