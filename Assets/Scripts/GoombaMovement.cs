using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : MonoBehaviour
{

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private bool alive = true;
    private Vector2 velocity;
    public Vector3 startPosition = new Vector3(-8.757f, -1.94f, 0.0f);

    private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
    private BoxCollider2D hitBox;
    private PolygonCollider2D hurtBox;
    private Animator goombaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        hitBox = GetComponent<BoxCollider2D>();
        hurtBox = GetComponent<PolygonCollider2D>();
        enemyBody = GetComponent<Rigidbody2D>();
        goombaAnimator = GetComponent<Animator>();
        // get the starting position
        originalX = transform.position.x;
        ComputeVelocity();
    }

    void Awake()
    {
        // other instructions
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    public void DeadGoomba()
    {
        alive = false;
    }

    // Update is called once per frame
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

    public void GameRestart()
    {
        goombaAnimator.SetTrigger("gameRestart");
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        hitBox.enabled = true;
        hurtBox.enabled = true;
        alive = true;
        ComputeVelocity();
    }
}
