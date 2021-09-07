using System.Collections;
using UnityEngine;

public class HomingMissilePowerup : Powerup
{
    protected override IEnumerator PowerupSequence()
    {
        Ship.GetComponent<ShipWeapons>().HomingMissilesAvailable = true;
        yield return new WaitForSeconds(0);
    }
}