using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            if (collision.gameObject.GetComponent<PlayerMovement>().baseSpeed >= 2.4f)
            {
                collision.gameObject.GetComponent<PlayerMovement>().baseSpeed = 2.5f;
            }
            else {
                collision.gameObject.GetComponent<PlayerMovement>().baseSpeed += 0.1f;
            }
            Destroy(gameObject);
        }
    }
}
