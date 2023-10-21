using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            collision.gameObject.GetComponent<PlayerMovement>().speedMultiplier *= 2;
            Destroy(gameObject);
        }
    }
}
