using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public int xcoord = 0;
    public int ycoord = 0;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            Destroy(gameObject);
        }
    }
}
