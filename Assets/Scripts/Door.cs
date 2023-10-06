using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    //public Vector3 exit;
    public GameObject floor;

    private void Start()
    {
        floor = GameObject.Find("Floor");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>()) {
            int x = Random.Range(0, 5);
            int y = Random.Range(0, 5);

            print(x + "," + y);
            floor.GetComponent<RoomGeneration>().telport(x, y);
        }
    }
    /*
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerMovement>()) {
            other.gameObject.transform.position = exit;
        }
    }
    */
}
