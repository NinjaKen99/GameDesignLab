using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Buff")]
public class BuffDecision : Decision
{
    public BuffTransformMap[] map;
    public override bool Decide(StateController controller)
    {
        BuffStateController m = (BuffStateController)controller;
        Buff toCompareState = EnumExtension.ParseEnum<Buff>(m.currentState.name);

        for (int i = 0; i < map.Length; i++)
        {
            if (toCompareState == map[i].fromState && m.currentPowerupType == map[i].powerupCollected)
            {
                return true;
            }
        }

        return false;
    }
}

[System.Serializable]
public struct BuffTransformMap
{
    public Buff fromState;
    public PowerupType powerupCollected;
}