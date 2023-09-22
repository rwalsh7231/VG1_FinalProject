using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGeneratingDoor : MonoBehaviour
{

    public bool contact = false;

    //if door is activated, where will the next room spawn?
    public char roomDir = 'L';

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>())
        {
            contact = true;
        }
    }
}
