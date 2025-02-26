using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup

    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        // other instructions
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerup.hasSpawned)
        {
            // show disabled sprite
            this.GetComponent<Animator>().SetTrigger("spawned");
            // spawn the powerup
            powerupAnimator.SetTrigger("spawned");
        }
    }
    public void GameRestart()
    {
        this.GetComponent<Animator>().SetTrigger("gameRestart");
        powerupAnimator.SetTrigger("gameRestart");
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        transform.localPosition = new Vector3(0, 0, 0);
        powerup.ResetPowerup();
    }

    // used by animator
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
