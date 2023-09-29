using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public int xcoord = 0;
    public int ycoord = 0;
    public Object blocker;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            if (other.gameObject != null) {
                GameObject floor = GameObject.Find("Floor");
                floor.GetComponent<RoomGeneration>().updateSpaces(xcoord, ycoord);
            }
            Destroy(gameObject);
        }

    }
}
