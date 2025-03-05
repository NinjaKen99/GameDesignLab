using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinPowerup : BasePowerup
{
    protected AudioSource spawnSFX;
    private int initialCoinCount;
    public int coinCount = 1;
    // public UnityEvent<int> increaseScore;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.Coin;
        rigidBody.bodyType = RigidbodyType2D.Static;
        spawnSFX = this.GetComponent<AudioSource>();
        boxCollider.enabled = false;
        initialCoinCount = coinCount;
        if (coinCount > 1) disable = false;
    }
    public override void GameRestart()
    {
        // base.GameRestart();
        coinCount = initialCoinCount;
        if (coinCount > 1) disable = false;
        spawned = false;
    }
    public override void SpawnPowerup()
    {
        spawnSFX.PlayOneShot(spawnSFX.clip);
        // increaseScore.Invoke(1);
        powerupCollected.Invoke(this);
        coinCount -= 1; // decrement coin count
        if (coinCount <= 1) disable = true;
        if (coinCount == 0) spawned = true; // Treat powerup as spawned only if coin count is 0
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

    }
}
