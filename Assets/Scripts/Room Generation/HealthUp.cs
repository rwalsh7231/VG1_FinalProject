using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    //in case we want to easily adjust healing amount
    public int healingAmount = 50;

    //gives the player health and then destroys itself
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>())
        {
            //heals the player if they need it
            if(collision.gameObject.GetComponent<PlayerMovement>().currHealth < collision.gameObject.GetComponent<PlayerMovement>().maxHealth) {
                collision.gameObject.GetComponent<PlayerMovement>().currHealth += healingAmount;
                collision.gameObject.GetComponent<PlayerMovement>().healthBar.fillAmount = collision.gameObject.GetComponent<PlayerMovement>().currHealth / collision.gameObject.GetComponent<PlayerMovement>().maxHealth;
                Destroy(gameObject);
            }
        }
    }
}
