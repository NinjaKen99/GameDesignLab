using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerupManager : MonoBehaviour
{
    public UnityEvent<IPowerup> powerupAffectsPlayer;
    public UnityEvent<IPowerup> powerupAffectsManager;

    public void FilterAndCastPowerup(IPowerup i)
    {
        switch (i.powerupType)
        {
            case PowerupType.Coin:
                powerupAffectsManager.Invoke(i);
                break;
            case PowerupType.OneUpMushroom:
                powerupAffectsManager.Invoke(i);
                break;
            case PowerupType.MagicMushroom:
                powerupAffectsPlayer.Invoke(i);
                break;
            case PowerupType.FireFlower:
                powerupAffectsPlayer.Invoke(i);
                break;
            case PowerupType.StarMan:
                powerupAffectsPlayer.Invoke(i);
                break;
            case PowerupType.Damage:
                powerupAffectsPlayer.Invoke(i);
                break;
        }
    }
}
