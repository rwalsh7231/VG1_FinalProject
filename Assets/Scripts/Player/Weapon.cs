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
                case 2:
                    //fire 8 projectiles with a spread
                    for (int i = 0; i < 8; i++) {
                        GameObject p = Instantiate(projectilePrefab);
                        p.transform.position = transform.position;
                        p.transform.rotation = rotation * Quaternion.Euler(0, 0, Random.Range(0, 20));
                    }
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
            case 2:
                this.ID = weapID;
                maxAmmo = 6;
                currAmmo = 6;
                GetComponent<SpriteRenderer>().sprite = weaponAppearances[1];
                break;
            default:
                break;
        }
    }
}
