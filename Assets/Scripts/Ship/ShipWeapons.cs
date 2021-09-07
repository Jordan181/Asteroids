using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipWeapons : MonoBehaviour
{
    private const float MainGunForce = 200f;

    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject homingMissilesSystemPrefab;

    private IReadOnlyDictionary<bool, Transform> firePoints;
    private IReadOnlyList<Transform> allFirePoints;
    private bool activeFirePoint;
    private bool firing;
    private bool firingHomingMissile;
    private bool homingMissilesAvailable;
    private GameObject homingMissilesSystem;
    private ShipController shipController;
    
    public bool UseBothGuns { get; set; }
    public bool RapidFireEnabled { get; set; }

    public bool HomingMissilesAvailable
    {
        get => homingMissilesAvailable;
        set
        {
            homingMissilesAvailable = value;

            if (homingMissilesAvailable)
            {
                if (homingMissilesSystem == null)
                    homingMissilesSystem = Instantiate(homingMissilesSystemPrefab);
            }
            else
            {
                homingMissilesSystem = null;
                firingHomingMissile = false;
            }
        }
    }

    private void Start()
    {
        shipController = GetComponent<ShipController>();

        firePoints = new Dictionary<bool, Transform>
        {
            { false, leftFirePoint},
            { true, rightFirePoint}
        };

        allFirePoints = firePoints.Values.ToList();

        UseBothGuns = false;
        RapidFireEnabled = false;
        HomingMissilesAvailable = false;
    }

    private void Update()
    {
        if (shipController.Destroyed)
            return;

        if (RapidFireEnabled)
        {
            firing = Input.GetButton(VirtualButtons.MainGunFire);
        }
        else if (!firing)
        {
            firing = Input.GetButtonDown(VirtualButtons.MainGunFire);
        }

        if (HomingMissilesAvailable)
        {
            if (!firingHomingMissile)
                firingHomingMissile = Input.GetButtonDown(VirtualButtons.MissileFire);
        }
    }

    private void FixedUpdate()
    {
        if (firing)
        {
            FireMainGun();
        }

        if (firingHomingMissile)
        {
            homingMissilesSystem.GetComponent<HomingMissileSystem>().Launch(allFirePoints);
            HomingMissilesAvailable = false;
        }
    }
    private void FireMainGun()
    {
        if (UseBothGuns)
        {
            foreach (var firePoint in allFirePoints)
            {
                FireBullet(firePoint);
            }
        }
        else
        {
            var firePoint = firePoints[activeFirePoint];
            FireBullet(firePoint);
            activeFirePoint = !activeFirePoint;
        }
        
        firing = false;
    }

    private void FireBullet(Transform firePoint)
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation)
            .GetComponent<Rigidbody2D>()
            .AddForce(firePoint.up * MainGunForce, ForceMode2D.Impulse);
    }
}
