using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public Object room;

    //it is likely we need a grid to keep track of used/not used spaces for generation
    //0 = not used, 1 = used
    int[,] spaces = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(0, 0, 0);
        Instantiate(room, position, Quaternion.identity);
        spaces[3, 3] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
