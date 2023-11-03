using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScriptTEST : MonoBehaviour
{
    public int xcoord = 0;
    public int ycoord = 0;
    GameObject player;
    GameObject floor;
    public GameObject eventItem;
    public List<Object> doors = new List<Object>();
    public bool contact = false;

    public List<Sprite> possibleSprites = new List<Sprite>();

    private void Start()
    {
        if (possibleSprites.Count > 0) {
            GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }

        player = GameObject.Find("Player");
        floor = GameObject.Find("Floor");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>()) {
            contact = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            contact = false;
        }
        /*
        //if the player and floor is not deleted, do the following
        if (player != null && floor != null) {
            floor.GetComponent<RoomGeneration>().updateSpaces(xcoord, ycoord);
            if(eventItem != null)
            {
                Destroy(eventItem);
            }
            Destroy(gameObject);
        }
        */
    }

    public void DestroyMe() {
        if (player != null && floor != null)
        {
            floor.GetComponent<RoomGeneration>().updateSpaces(xcoord, ycoord);
            if (eventItem != null)
            {
                Destroy(eventItem);
            }

            for(int i = 0; i < doors.Count; i++)
            {
                Destroy(doors[i]);
            }
            doors.Clear();

            Destroy(gameObject);
        }
    }
}
