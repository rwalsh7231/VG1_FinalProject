using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigRoom : MonoBehaviour
{
    public List<int[]> coords = new List<int[]>();
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
        if (player != null && floor != null)
        {
            //for each coordinate this big room takes up, reactivate the blocker
            for(int i = 0; i < coords.Count; i++)
            {
                floor.GetComponent<RoomGeneration>().updateSpaces(coords[i][0], coords[i][1]);
            }

            if (eventItem != null)
            {
                Destroy(eventItem);
            }
            Destroy(gameObject);
        }
    }
}
