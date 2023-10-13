using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Transform player;

    // Update is called once per frame
    void Update()
    {
        //since we are upsizing the grid, now the camera will follow the player
        transform.position = player.transform.position + new Vector3(0, 0, -10);
    }
}
