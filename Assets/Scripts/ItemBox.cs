using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public int coinCount = 1;
    public AudioSource bumpAudio;
    public AudioClip coinAudio;
    public Animator blockAnimator;

    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        blockAnimator.SetInteger("coinCount", coinCount);
    }

    // Check for collision with edge collider at bottom
    void OnCollisionEnter2D(Collision2D col)
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, maxDistance, layerMask))
        {
            Debug.Log("Hit from below");
            blockAnimator.SetTrigger("onHit");
        }
    }

    // Decrease Animator coin count
    void coin()
    {
        blockAnimator.SetInteger("coinCount", blockAnimator.GetInteger("coinCount") - 1);
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
