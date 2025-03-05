using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    // public void GoombaStomp(GameObject goomba)
    // {
    //     // goomba.GetComponent<BoxCollider2D>().enabled = false;
    //     // goomba.GetComponent<PolygonCollider2D>().enabled = false;
    //     goomba.GetComponent<Animator>().SetTrigger("onStomp");
    //     goomba.GetComponent<GoombaMovement>().PlayDeathSound();
    // }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<GoombaMovement>().GameRestart();
        }
    }
}
