using System.Collections;
using UnityEngine;

public class DoubleGunPowerup : Powerup
{
    [SerializeField] private float duration;

    protected override IEnumerator PowerupSequence()
    {
        var shipWeapons = Ship.GetComponent<ShipWeapons>();

        shipWeapons.UseBothGuns = true;

        yield return new WaitForSeconds(duration);

        shipWeapons.UseBothGuns = false;

        Destroy(gameObject);
    }
}
