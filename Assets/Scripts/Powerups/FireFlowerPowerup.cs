using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerPowerup : BasePowerup
{
    protected AudioSource spawnSFX;
    public AudioClip powerupSFX;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.FireFlower;
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
    }
    public override void SpawnPowerup()
    {
        spawned = true;
        // rigidBody.bodyType = RigidbodyType2D.Dynamic;
        boxCollider.enabled = true;
        spawnSFX.PlayOneShot(spawnSFX.clip);
        // rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
    }
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
}
