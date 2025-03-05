using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStarman")]

public class StartStarmanAction : Action
{
    public AudioClip starmanStart;
    public override void Act(StateController controller)
    {
        BuffStateController m = (BuffStateController)controller;
        m.gameObject.GetComponent<AudioSource>().PlayOneShot(starmanStart);
        m.SetRendererToFlash();
    }
}
