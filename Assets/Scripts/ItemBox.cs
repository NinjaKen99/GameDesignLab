using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public int initialCoinCount = 1;
    protected int coinCount;
    public AudioSource bumpAudio;
    public AudioClip coinAudio;
    public Animator blockAnimator;

    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        coinCount = initialCoinCount;
        blockAnimator.SetInteger("coinCount", coinCount);
    }

    // void Awake()
    // {
    //     // other instructions
    //     GameManager.instance.gameRestart.AddListener(GameRestart);
    // }

    // Check for collision with edge collider at bottom
    void OnCollisionEnter2D(Collision2D col)
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            // Debug.Log("Hit from below");
            blockAnimator.SetTrigger("onHit");
        }
    }

    public void GameRestart()
    {
        blockAnimator.SetTrigger("gameRestart");
        coinCount = initialCoinCount;
        blockAnimator.SetInteger("coinCount", coinCount);
    }

    // Decrease Animator coin count
    void coin()
    {
        coinCount -= 1;
        blockAnimator.SetInteger("coinCount", coinCount);
    }

    void PlayCoinSound()
    {
        bumpAudio.PlayOneShot(coinAudio);
    }

    void PlayBumpSound()
    {
        bumpAudio.PlayOneShot(bumpAudio.clip);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
}
