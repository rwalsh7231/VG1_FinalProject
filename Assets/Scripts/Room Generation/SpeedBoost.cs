using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            collision.gameObject.GetComponent<PlayerMovement>().baseSpeed += 0.5f;
            Destroy(gameObject);
        }
    }
}
