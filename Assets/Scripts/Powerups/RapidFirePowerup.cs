using System.Collections;
using UnityEngine;

public class RapidFirePowerup : Powerup
{
    [SerializeField] private float duration;

    protected override IEnumerator PowerupSequence()
    {
        var shipWeapons = Ship.GetComponent<ShipWeapons>();

        shipWeapons.RapidFireEnabled = true;

        yield return new WaitForSeconds(duration);

        shipWeapons.RapidFireEnabled = false;

        Destroy(gameObject);
    }
}
