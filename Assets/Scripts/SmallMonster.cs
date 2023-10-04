using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonster : MonoBehaviour
{
    public Transform player;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<PlayerMovement>()) {
            PlayerMovement.instance.currHealth = PlayerMovement.instance.currHealth - 1;
            Destroy(gameObject);
        }
        if(other.gameObject.GetComponent<Projectile>()) {
            Destroy(gameObject);
            PlayerMovement.instance.EarnPoints(10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDir = player.position - transform.position;
        playerDir.Normalize();
        transform.position += (playerDir * Time.deltaTime)/10;
    }
}
