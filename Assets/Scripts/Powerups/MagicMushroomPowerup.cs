using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    // setup this object's type
    // instantiate variables

    protected AudioSource spawnSFX;
    public AudioClip powerupSFX;

    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
        rigidBody.bodyType = RigidbodyType2D.Static;
        spawnSFX = this.GetComponent<AudioSource>();
        boxCollider.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            Debug.Log("Contact with Powerup");
            spawnSFX.PlayOneShot(powerupSFX);
            // then destroy powerup (optional)
            powerupAnimator.SetTrigger("consumed");
            powerupCollected.Invoke(this);
            DestroyPowerup();

        }
        else if (col.gameObject.layer == 11) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

            }
        }
        if (rigidBody.velocity.x == 0)
        {
            rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
    }
    public override void SpawnPowerup()
    {
        spawned = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(MovePowerup());
        boxCollider.enabled = true;
        spawnSFX.PlayOneShot(spawnSFX.clip);
        // rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }
    private IEnumerator MovePowerup()
    {
        Debug.Log("Wait for animation");
        yield return new WaitForSeconds(0.55f); // wait for length of spawn animation
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // base.ApplyPowerup(i);
        // try
        MarioStateController mario;
        bool result = i.TryGetComponent<MarioStateController>(out mario);
        if (result)
        {
            mario.SetPowerup(this.powerupType);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
