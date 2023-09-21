using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    public Object room;
    public Object door;

    private Object currentRoom;

    //it is likely we need a grid to keep track of used/not used spaces for generation
    //0 = not used, 1 = used
    int[,] spaces = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = new Vector3(0, 0, 0);
        currentRoom = Instantiate(room, position, Quaternion.identity);
        spaces[3, 3] = 1;
        generateDoor();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generateDoor() {
        int howMany = Random.Range(2, 4);
        List<char> directions = new List<char> { 'U', 'D', 'L', 'D'};

        for(int i = 0; i < howMany; i++)
        {
            int index = Random.Range(0, directions.Count);
            char direction = directions[index];

            Vector3 position = currentRoom.GetComponent<Transform>().position;

            switch (direction){
                case 'U':
                    position.y += 0.5f;
                    Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    break;
                case 'D':
                    position.y -= 0.5f;
                    Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    break;
                case 'L':
                    position.x -= 0.5f;
                    Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                    break;
                case 'R':
                    position.x -= 0.5f;
                    Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                    break;
                default:
                    break;
            }
            directions.Remove(direction);
        }
    }

    void newRoom() { 
        
    }
}
