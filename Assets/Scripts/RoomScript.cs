using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public int xcoord = 0;
    public int ycoord = 0;
    GameObject player;
    GameObject floor;
    public GameObject eventItem;

    private void Start()
    {
        player = GameObject.Find("Player");
        floor = GameObject.Find("Floor");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if the player and floor is not deleted, do the following
        if (player != null && floor != null) {
            floor.GetComponent<RoomGeneration>().updateSpaces(xcoord, ycoord);
            if(eventItem != null)
            {
                Destroy(eventItem);
            }
            Destroy(gameObject);
        }
    }
}
