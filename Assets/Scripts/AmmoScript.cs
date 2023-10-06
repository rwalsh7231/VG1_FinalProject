using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{
    //gives the player ammo and then destroys itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            collision.gameObject.GetComponent<PlayerMovement>().currAmmo += 5;
            Destroy(gameObject);
        }
    }
}
