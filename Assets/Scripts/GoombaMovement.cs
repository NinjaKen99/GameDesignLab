using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoombaMovement : MonoBehaviour
{
    public GameConstants gameConstants;
    private float originalX;
    public float maxOffset;
    private float enemyPatroltime;

    private bool alive = true;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;

    private Vector3 localStartPosition;

    private AudioSource audioSource;

    // events invoked by Enemies
    public UnityEvent damagePlayer;
    public UnityEvent<int> increaseScore;

    void Awake()
    {
        // other instructions
        // GameManager.instance.gameRestart.AddListener(GameRestart);
        localStartPosition = this.transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set constants
        if (maxOffset == 0) maxOffset = gameConstants.goombaMaxOffset;
        enemyPatroltime = gameConstants.goombaPatrolTime;

        audioSource = GetComponent<AudioSource>();
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (alive)
        {
            if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
            {// move goomba
                Movegoomba();
            }
            else
            {
                // change direction
                moveRight *= -1;
                ComputeVelocity();
                Movegoomba();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            State marioBuffState = other.gameObject.GetComponent<BuffStateController>().currentState;
            if (string.Equals(marioBuffState.name, "Invincible", System.StringComparison.OrdinalIgnoreCase))
            {
                increaseScore.Invoke(1);
                GetComponent<Animator>().SetTrigger("onStomp");
            }
            else
            {
                float enemyMarioOffset = other.transform.position.y - transform.position.y;
                float enemySpriteHeight = this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
                float marioSpriteHeight = other.gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
                if (enemyMarioOffset > marioSpriteHeight / 2)
                {
                    increaseScore.Invoke(1);
                    GetComponent<Animator>().SetTrigger("onStomp");
                }
                else
                {
                    damagePlayer.Invoke();
                }
            }
        }
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     // If colllide with Pipe or Enemy, change direction
    //     if (col.gameObject.layer == 11 || col.gameObject.layer == 6)
    //     {
    //         moveRight *= -1;
    //         ComputeVelocity();
    //     }
    // }

    public void PlayDeathSound()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<PolygonCollider2D>().enabled = false;
        alive = false;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void GameRestart()
    {
        this.GetComponent<BoxCollider2D>().enabled = true;
        this.GetComponent<PolygonCollider2D>().enabled = true;
        this.GetComponent<Animator>().SetTrigger("gameRestart");
        alive = true;
        transform.localPosition = localStartPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }
}
