using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff
{
    Default = -1,
    Invincible = 0
}

public class BuffStateController : StateController
{
    public PowerupType currentPowerupType = PowerupType.Default;
    public Buff shouldBeNextState = Buff.Default;
    public GameConstants gameConstants;
    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private bool toggle = true;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }
    // this should be added to the GameRestart EventListener as callback
    public void GameRestart()
    {
        // clear powerup
        currentPowerupType = PowerupType.Default;
        // set the start state
        TransitionToState(startState);
    }
    public void SetPowerup(PowerupType i)
    {
        currentPowerupType = i;
    }
    public void ExitStarman()
    {
        currentPowerupType = PowerupType.Default;
        TransitionToState(startState);
    }

    public void SetRendererToFlash()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FlashSpriteRenderer());
    }
    private IEnumerator FlashSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialColor = spriteRenderer.color;
        while (string.Equals(currentState.name, "Invincible", System.StringComparison.OrdinalIgnoreCase))
        {
            toggle = !toggle;
            // Toggle between 2 colors of sprite
            spriteRenderer.color = toggle ? gameConstants.orange : gameConstants.red;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(gameConstants.flickerInterval * 100);
        }
        // Reset to default colour
        spriteRenderer.color = initialColor;
        ExitStarman();
    }
}
