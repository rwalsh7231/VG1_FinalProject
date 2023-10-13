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
    public GameObject room;
    public GameObject bigRoom;
    public GameObject door;
    public GameObject blocker;
    public GameObject ammoCrate;
    public GameObject Teleporter;

    //keep track of the room that the player is currently in and the doors that are currently active
    private GameObject currentRoom;
    private GameObject prevRoom;
    private char prevRoomDir;
    private List<Object> activeDoors = new List<Object>();

    //it is likely we need a grid to keep track of used/not used spaces for generation
    GameObject[,] spaces = new GameObject[15, 15];

    // Start is called before the first frame update
    void Start()
    {
        //make a new room, place it in the middle of the grid, and generate some doors
        Vector3 position = new Vector3(0, 0, 0);
        currentRoom = Instantiate(room, position, Quaternion.identity);
        currentRoom.GetComponent<RoomScript>().xcoord = 0;
        currentRoom.GetComponent<RoomScript>().ycoord = 0;

        
        //fill the grid with blockers
        for (int i = -7; i <= 7; i++) {
            for (int j = -7; j <= 7; j++) {
                position = new Vector3(i*2, j*2, 0);
                GameObject block = Instantiate(blocker, position, Quaternion.identity);
                spaces[i + 7, j + 7] = block;
            }
        }

        spaces[7, 7].SetActive(false);
        
        //since this is the first room, there is no exclusion
        generateDoor('X');

    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoom == null) {
            currentRoom = prevRoom;
            for(int i = 0; i < activeDoors.Count; i++)
            {
                Destroy(activeDoors[i]);
            }
            activeDoors.Clear();

            generateDoor(prevRoomDir);
        }

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
        if (currentRoom.GetComponent<RoomScript>().xcoord == -7) {
            directions.Remove('L');
        }

        if (currentRoom.GetComponent<RoomScript>().xcoord == 7)
        {
            directions.Remove('R');
        }

        if (currentRoom.GetComponent<RoomScript>().ycoord == -7)
        {
            directions.Remove('D');
        }

        if (currentRoom.GetComponent<RoomScript>().ycoord == 7)
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
                    position.y += 0.95f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'D':
                    position.y -= 0.95f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'L':
                    position.x -= 0.95f;
                    genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                    genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                    activeDoors.Add(genDoor);
                    break;
                case 'R':
                    position.x += 0.95f;
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

        GameObject newRoom = null;

        //which direction should a room generate?
        switch (d.GetComponent<RoomGeneratingDoor>().roomDir) {
            case 'U':
                position.y += 2;
                newRoom = (GameObject) Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().ycoord = currentRoom.GetComponent<RoomScript>().ycoord + 1;
                prevRoomDir = 'D';
                break;
            case 'D':
                position.y -= 2;
                newRoom = (GameObject) Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().ycoord = currentRoom.GetComponent<RoomScript>().ycoord - 1;
                prevRoomDir = 'U';
                break;
            case 'L':
                position.x -= 2;
                newRoom = (GameObject) Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().xcoord = currentRoom.GetComponent<RoomScript>().xcoord - 1;
                prevRoomDir = 'R';
                break;
            case 'R':
                position.x += 2;
                newRoom = (GameObject) Instantiate(currentRoom, position, Quaternion.identity);
                newRoom.GetComponent<RoomScript>().xcoord = currentRoom.GetComponent<RoomScript>().xcoord + 1;
                prevRoomDir = 'L';
                break;
            default:
                break;
        }

        for(int i = 0; i < activeDoors.Count; i++)
        {
            Destroy(activeDoors[i]);
        }

        //deactivate room
        spaces[newRoom.GetComponent<RoomScript>().xcoord + 7, newRoom.GetComponent<RoomScript>().ycoord + 7].SetActive(false);

        //spaces[(int)position.x + 2, (int)position.y + 2] = newRoom;

        activeDoors.Clear();

        //current room becomes the new room
        prevRoom = currentRoom;
        currentRoom = newRoom;

        createEvent(position);

        //make some new rooms
        generateDoor(d.GetComponent<RoomGeneratingDoor>().roomDir);

    }

    public void updateSpaces(int x, int y) {

        //check if the space has been deleted, if not make it active
        if (spaces[x+7, y+7] != null)
        {
            spaces[x + 7, y + 7].SetActive(true);
        }
        
    }

    //has a chance to generate a room event
    public void createEvent(Vector3 position) {
        int num = Random.Range(0, 10);

        if (num == 0) {
            GameObject Ammo = Instantiate(ammoCrate, position, Quaternion.identity);
            currentRoom.GetComponent<RoomScript>().eventItem = Ammo;
        }

        if (num == 1) { 
            GameObject Tele = Instantiate(Teleporter, position, Quaternion.identity);
            currentRoom.GetComponent<RoomScript>().eventItem = Tele;
        }
        
    }

    public void telport(int x, int y)
    {
        //restart blockers and delete rooms
        spaces[currentRoom.GetComponent<RoomScript>().xcoord+7, currentRoom.GetComponent<RoomScript>().ycoord+7].SetActive(true);
        Destroy(currentRoom);
        if(prevRoom != null)
        {
            spaces[prevRoom.GetComponent<RoomScript>().xcoord + 7, prevRoom.GetComponent<RoomScript>().ycoord + 7].SetActive(true);
            Destroy(prevRoom);
        }

        //destroy all currently active doors
        for (int i = 0; i < activeDoors.Count; i++) {
            Destroy(activeDoors[i]);
        }
        activeDoors.Clear();

        //open a new space
        spaces[x, y].SetActive(false);

        Vector3 position = new Vector3(2*(x-7), 2*(y-7), 0);

        currentRoom = Instantiate(room, position, Quaternion.identity);
        currentRoom.GetComponent<RoomScript>().xcoord = x-7;
        currentRoom.GetComponent<RoomScript>().ycoord = y-7;
        GameObject player = GameObject.Find("Player");
        player.transform.position = position;

        generateDoor('X');
    }
}
