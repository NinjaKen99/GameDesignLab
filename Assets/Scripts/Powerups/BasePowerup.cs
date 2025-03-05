using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePowerup : MonoBehaviour, IPowerup
{
    public PowerupType type;
    public bool spawned = false;
    public bool disable = true;
    protected bool goRight = true;
    protected Rigidbody2D rigidBody;
    protected BoxCollider2D boxCollider;
    protected Vector3 startPosition;
    public Animator powerupAnimator;
    public UnityEvent<IPowerup> powerupCollected;

    // base methods
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        startPosition = transform.localPosition;
    }

    // interface methods
    // 1. concrete methods
    public PowerupType powerupType
    {
        get // getter
        {
            return type;
        }
    }

    public bool hasSpawned
    {
        get // getter
        {
            return spawned;
        }
    }

    public bool toDisable
    {
        get // getter
        {
            return disable;
        }
    }

    public void DestroyPowerup()
    {
        // Destroy(this.gameObject);
        // this.gameObject.SetActive(false);
        rigidBody.bodyType = RigidbodyType2D.Static;
        boxCollider.enabled = false;
        transform.localPosition = startPosition;
    }

    public virtual void GameRestart()
    {
        // Move to original location
        rigidBody.bodyType = RigidbodyType2D.Static;
        transform.localPosition = startPosition;
        boxCollider.enabled = false;
        // Reset boolean anim trigger
        spawned = false;
    }

    // 2. abstract methods, must be implemented by derived classes
    // public abstract void SpawnPowerup();
    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);
}
