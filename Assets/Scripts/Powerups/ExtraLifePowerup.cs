using System.Collections;
using UnityEngine;

public class ExtraLifePowerup : Powerup
{
    protected override IEnumerator PowerupSequence()
    {
        PlayerStats.Lives++;
        yield return new WaitForSeconds(0);
    }
}
