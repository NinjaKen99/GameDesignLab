using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public AudioSource stompAudio;
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

    public void GoombaStomp(GameObject goomba)
    {
        goomba.GetComponent<BoxCollider2D>().enabled = false;
        goomba.GetComponent<PolygonCollider2D>().enabled = false;
        goomba.GetComponent<Animator>().SetTrigger("onStomp");
        stompAudio.PlayOneShot(stompAudio.clip);
        goomba.GetComponent<GoombaMovement>().DeadGoomba();
    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<GoombaMovement>().GameRestart();
        }
    }
}
