using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RoomGeneration : MonoBehaviour
{
    //we need to keep track of rooms and doors this object can generate
    public Object room;
    public Object door;

    //keep track of the room that the player is currently in and the doors that are currently active
    private Object currentRoom;
    private List<Object> activeDoors = new List<Object>();

    //it is likely we need a grid to keep track of used/not used spaces for generation
    //0 = not used, 1 = used
    int[,] spaces = { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };

    // Start is called before the first frame update
    void Start()
    {
        //make a new room, place it in the middle of the grid, and generate some doors
        Vector3 position = new Vector3(0, 0, 0);
        currentRoom = Instantiate(room, position, Quaternion.identity);
        currentRoom.GetComponent<RoomScript>().xcoord = 3;
        currentRoom.GetComponent<RoomScript>().ycoord = 3;
        spaces[3, 3] = 1;
        //since this is the first room, there is no exclusion
        generateDoor('X');

    }

    // Update is called once per frame
    void Update()
    {
        //checks each frame to see if a door has been touched by a plyer.
        //if so, make a new room
        for (int i = 0; i < activeDoors.Count; i++) {
            if (activeDoors[i] != null && activeDoors[i].GetComponent<RoomGeneratingDoor>().contact) {
                print("size of active doors " + activeDoors.Count);
                print(i);
                newRoom(activeDoors[i]);
                return;
            }
        }
    }

    void generateDoor(char exclusion) {
        //first we need to know how many rooms can be generated
        
        //four directions
        List<char> directions = new List<char> { 'U', 'D', 'L', 'R'};

        //Avoid bugs and don't add a new door where player entered
        switch (exclusion) {
            case 'U':
                directions.Remove('D');
                break;
            case 'D':
                directions.Remove('U');
                break;
            case 'L':
                directions.Remove('R');
                break;
            case 'R':
                directions.Remove('L');
                break;
            default:
                break;
        }

        //don't go off grid, remove any directions that are impossible
        if (currentRoom.GetComponent<RoomScript>().xcoord == 1) {
            directions.Remove('L');
        }

        if (currentRoom.GetComponent<RoomScript>().xcoord == 5)
        {
            directions.Remove('R');
        }

        if (currentRoom.GetComponent<RoomScript>().ycoord == 1)
        {
            directions.Remove('D');
        }

        if (currentRoom.GetComponent<RoomScript>().ycoord == 5)
        {
            directions.Remove('U');
        }


        int howMany = Random.Range(1, directions.Count);

        //for each room to be generated
        for (int i = 0; i < howMany; i++)
        {
            //get a random direction
            int index = Random.Range(0, directions.Count);
            char direction = directions[index];

            //get the position of the current room
            Vector3 position = currentRoom.GetComponent<Transform>().position;

            //a placeholder for each door to be generated
            Object genDoor;

            //place the new door in a specific position and save the door to the activeDoor list
            switch (direction){
                case 'U':
                    position.y += 0.45f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'D':
                    position.y -= 0.45f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'L':
                    position.x -= 0.45f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'R':
                    position.x += 0.45f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                default:
                    break;
            }
            directions.Remove(direction);
        }
    }

    public void newRoom(Object d) {

        //get the position of the current room
        Vector3 position = currentRoom.GetComponent<Transform>().position;

        Object newRoom = null;

        switch (d.GetComponent<RoomGeneratingDoor>().roomDir) {
            case 'U':
                position.y += 1;
                newRoom = Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().ycoord = currentRoom.GetComponent<RoomScript>().ycoord + 1;
                break;
            case 'D':
                position.y -= 1;
                newRoom = Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().ycoord = currentRoom.GetComponent<RoomScript>().ycoord - 1;
                break;
            case 'L':
                position.x -= 1;
                newRoom = Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().xcoord = currentRoom.GetComponent<RoomScript>().xcoord - 1;
                break;
            case 'R':
                position.x += 1;
                newRoom = Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().xcoord = currentRoom.GetComponent<RoomScript>().xcoord + 1;
                break;
            default:
                break;
        }

        for(int i = 0; i < activeDoors.Count; i++)
        {
            Destroy(activeDoors[i]);
        }

        activeDoors.Clear();

        Destroy(currentRoom);

        currentRoom = newRoom;

        generateDoor(d.GetComponent<RoomGeneratingDoor>().roomDir);

    }
}
