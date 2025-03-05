using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller for Animation based blocks
public class BrickPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerup.hasSpawned) // Collision by Player, not spawned check
        {
            if (powerup.toDisable) // If next state is disabled
            {
                this.GetComponent<Animator>().SetTrigger("spawned");
            }
            else
            {
                this.GetComponent<Animator>().SetTrigger("onHit");
            }
            powerupAnimator.SetTrigger("spawned");
        }
    }
    public void GameRestart()
    {
        this.GetComponent<Animator>().SetTrigger("gameRestart");
        if (powerup.type != PowerupType.Coin) powerupAnimator.SetTrigger("gameRestart");
    }
    public void Disable()
    {

    }
}
