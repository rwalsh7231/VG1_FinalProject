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

    //ID determines weapon behaviour when used
    public int ID;

    // Start is called before the first frame update
    void Start()
    {
        weaponPickup(1);
    }

    public void fireWeapon(Quaternion rotation) {
        if (currAmmo > 0) {
            currAmmo--;

            switch (ID) {
                case 1:
                    GameObject newProjectile = Instantiate(projectilePrefab);
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
                break;
            default:
                break;
        }
    }
}
