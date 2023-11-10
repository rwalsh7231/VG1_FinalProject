using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    int ID = 1;
    public List<Sprite> appearances;

    // Start is called before the first frame update
    void Start()
    {
        if (appearances.Count > 0)
        {
            int randNum = Random.Range(0, appearances.Count);
            GetComponent<SpriteRenderer>().sprite = appearances[randNum];
            ID = randNum;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //get player's gun and give it half of its max ammo
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            Weapon playerWeapon = GameObject.Find("Gun").GetComponent<Weapon>();

            playerWeapon.weaponPickup(ID);

            Destroy(gameObject);
        }
    }
}
