using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    //gives the player ammo and then destroys itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get player's gun and give it half of its max ammo
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            Weapon playerWeapon = GameObject.Find("Gun").GetComponent<Weapon>();

            if (playerWeapon.currAmmo != playerWeapon.maxAmmo) {
                playerWeapon.currAmmo += (playerWeapon.maxAmmo / 2);

                if (playerWeapon.currAmmo > playerWeapon.maxAmmo)
                {
                    playerWeapon.currAmmo = playerWeapon.maxAmmo;
                }

                Destroy(gameObject);
            }
        }
    }
}
