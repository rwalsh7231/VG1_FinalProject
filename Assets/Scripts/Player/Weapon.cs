using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //what does the weapon look like?
    public List<Sprite> weaponAppearances;

    //need something to fire
    public GameObject projectilePrefab;

    //how much ammo does the weapon have
    public int currAmmo;
    public int maxAmmo;

    public float fireDelay = 1f;
    private float fireWait = 0;

    //ID determines weapon behaviour when used
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        weaponPickup(1);
    }

    private void Update()
    {
        fireWait += Time.deltaTime;
    }

    public void fireWeapon(Quaternion rotation) {
        if (currAmmo > 0 && fireWait > fireDelay)
        {
            currAmmo--;

            fireWait = 0;

            GameObject newProjectile;

            switch (ID)
            {
                case 1:
                    newProjectile = Instantiate(projectilePrefab);
                    newProjectile.transform.position = transform.position;
                    newProjectile.transform.rotation = rotation;
                    break;
                case 2:
                    //fire 8 projectiles with a spread
                    for (int i = 0; i < 8; i++)
                    {
                        newProjectile = Instantiate(projectilePrefab);
                        newProjectile.transform.position = transform.position;
                        newProjectile.transform.rotation = rotation * Quaternion.Euler(0, 0, Random.Range(0, 20));
                    }
                    break;
                case 3:
                    newProjectile = Instantiate(projectilePrefab);
                    newProjectile.transform.position = transform.position;
                    newProjectile.transform.rotation = rotation;
                    break;
                default:
                    break;
            }
        }
    }

    public void weaponPickup(int weapID) {
        switch (weapID) {
            case 1:
                this.ID = weapID;
                maxAmmo = 10;
                currAmmo = 10;
                GetComponent<SpriteRenderer>().sprite = weaponAppearances[0];
                fireDelay = 0.5f;
                break;
            case 2:
                this.ID = weapID;
                maxAmmo = 6;
                currAmmo = 6;
                GetComponent<SpriteRenderer>().sprite = weaponAppearances[1];
                fireDelay = 1f;
                break;
            case 3:
                this.ID = weapID;
                maxAmmo = 30;
                currAmmo = 30;
                GetComponent<SpriteRenderer>().sprite = weaponAppearances[2];
                fireDelay = 0.1f;
                break;
            default:
                break;
        }
    }
}
