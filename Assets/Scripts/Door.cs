using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    public Vector3 exit;
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerMovement>()) {
            other.gameObject.transform.position = exit;
        }
    }
}
