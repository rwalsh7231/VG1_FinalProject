using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RoomGenerationTEST : MonoBehaviour
{
    //we need to keep track of rooms and doors this object can generate
    public GameObject room;
    public GameObject door;
    public GameObject blocker;
    public GameObject ammoCrate;
    public GameObject Teleporter;
    public GameObject eventMonster;
    public GameObject healthUp;

    public GameObject SpeedBoost;

    //keep track of the room that the player is currently in and the doors that are currently active
    public List<GameObject> currentRoom = new List<GameObject>();
    public List<GameObject> prevRoom = new List<GameObject>();
    private char prevRoomDir;

    //it is likely we need a grid to keep track of used/not used spaces for generation
    GameObject[,] spaces = new GameObject[15, 15];

    // Start is called before the first frame update
    void Start()
    {
        //make a new room, place it in the middle of the grid, and generate some doors
        Vector3 position = new Vector3(0, 0, 0);
        currentRoom.Add(Instantiate(room, position, Quaternion.identity)); //first room will always be 1x1
        currentRoom[0].GetComponent<RoomScript>().xcoord = 0;
        currentRoom[0].GetComponent<RoomScript>().ycoord = 0;

        
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

    void CheckForNoContact(List<GameObject> room) {
        if (room.Count > 0)
        {
            bool noContact = true;
            for (int i = 0; i < room.Count; i++)
            {
                if (room[i] != null && room[i].GetComponent<RoomScriptTEST>().contact)
                {
                    noContact = false;
                }
            }

            if (noContact)
            {
                for (int i = 0; i < room.Count; i++)
                {
                    room[i].GetComponent<RoomScriptTEST>().DestroyMe();
                }
                room.Clear();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRoom.Count == 0) {
            currentRoom = prevRoom;

            generateDoor(prevRoomDir);
        }

        if (currentRoom.Count > 0)
        {
            bool noContact = true;
            for (int i = 0; i < currentRoom.Count; i++)
            {
                if (currentRoom[i] != null && currentRoom[i].GetComponent<RoomScriptTEST>().contact)
                {
                    noContact = false;
                }
            }

            if (noContact)
            {
                for (int i = 0; i < currentRoom.Count; i++)
                {
                    currentRoom[i].GetComponent<RoomScriptTEST>().DestroyMe();
                }
                currentRoom.Clear();
            }
        }

        if (prevRoom.Count > 0) {
            bool noContact = true;
            for (int i = 0; i < prevRoom.Count; i++)
            {
                if (prevRoom[i] != null && prevRoom[i].GetComponent<RoomScriptTEST>().contact)
                {
                    noContact = false;
                }
            }

            if (noContact)
            {
                for (int i = 0; i < prevRoom.Count; i++)
                {
                    prevRoom[i].GetComponent<RoomScriptTEST>().DestroyMe();
                }
                prevRoom.Clear();
            }
        }

        //checks each frame to see if a door has been touched by a plyer.
        //if so, make a new room
        for(int i = 0; i < currentRoom.Count; i++)
        {
            for (int j = 0; j < currentRoom[i].GetComponent<RoomScriptTEST>().doors.Count; j++) {
                if (currentRoom[i].GetComponent<RoomScriptTEST>().doors[j] != null && currentRoom[i].GetComponent<RoomScriptTEST>().doors[j].GetComponent<RoomGeneratingDoor>().contact) {
                    newRoom(currentRoom[i].GetComponent<RoomScriptTEST>().doors[j]);
                }
            }
        }
    }


    void generateDoor(char exclusion) {
        //first we need to know how many rooms can be generated

        switch (currentRoom.Count) {
            case 1:
                //four directions
                List<char> directions = new List<char> { 'U', 'D', 'L', 'R' };

                //Avoid bugs and don't add a new door where player entered
                switch (exclusion)
                {
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
                if (currentRoom[0].GetComponent<RoomScriptTEST>().xcoord == -7)
                {
                    directions.Remove('L');
                }

                if (currentRoom[0].GetComponent<RoomScriptTEST>().xcoord == 7)
                {
                    directions.Remove('R');
                }

                if (currentRoom[0].GetComponent<RoomScriptTEST>().ycoord == -7)
                {
                    directions.Remove('D');
                }

                if (currentRoom[0].GetComponent<RoomScriptTEST>().ycoord == 7)
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
                    Vector3 position = currentRoom[0].GetComponent<Transform>().position;

                    //a placeholder for each door to be generated
                    Object genDoor;

                    //place the new door in a specific position and save the door to the activeDoor list
                    switch (direction)
                    {
                        case 'U':
                            position.y += 0.95f;
                            genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                            genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                            currentRoom[0].GetComponent<RoomScriptTEST>().doors.Add(genDoor);
                            break;
                        case 'D':
                            position.y -= 0.95f;
                            genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 90));
                            genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                            currentRoom[0].GetComponent<RoomScriptTEST>().doors.Add(genDoor);
                            break;
                        case 'L':
                            position.x -= 0.95f;
                            genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                            genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                            currentRoom[0].GetComponent<RoomScriptTEST>().doors.Add(genDoor);
                            break;
                        case 'R':
                            position.x += 0.95f;
                            genDoor = Instantiate(door, position, Quaternion.Euler(0, 0, 0));
                            genDoor.GetComponent<RoomGeneratingDoor>().roomDir = direction;
                            currentRoom[0].GetComponent<RoomScriptTEST>().doors.Add(genDoor);
                            break;
                        default:
                            break;
                    }
                    directions.Remove(direction);
                }
                break;
            case 4:
                break;
            default:
                break;
        }

        
    }

    public void newRoom(Object d) {

        switch (currentRoom.Count) {
            case 1:
                //get the position of the current room
                Vector3 position = currentRoom[0].GetComponent<Transform>().position;

             GameObject newRoom = null;

                    //which direction should a normal room generate?
            switch (d.GetComponent<RoomGeneratingDoor>().roomDir)
            {
                case 'U':
                    position.y += 2;
                    newRoom = (GameObject)Instantiate(currentRoom[0], position, Quaternion.identity);
                    newRoom.GetComponent<RoomScriptTEST>().ycoord = currentRoom[0].GetComponent<RoomScriptTEST>().ycoord + 1;
                    prevRoomDir = 'D';
                    break;
                case 'D':
                    position.y -= 2;
                    newRoom = (GameObject)Instantiate(currentRoom[0], position, Quaternion.identity);
                    newRoom.GetComponent<RoomScriptTEST>().ycoord = currentRoom[0].GetComponent<RoomScriptTEST>().ycoord - 1;
                    prevRoomDir = 'U';
                    break;
                case 'L':
                    position.x -= 2;
                    newRoom = (GameObject)Instantiate(currentRoom[0], position, Quaternion.identity);
                    newRoom.GetComponent<RoomScriptTEST>().xcoord = currentRoom[0].GetComponent<RoomScriptTEST>().xcoord - 1;
                    prevRoomDir = 'R';
                    break;
                case 'R':
                    position.x += 2;
                    newRoom = (GameObject)Instantiate(currentRoom[0], position, Quaternion.identity);
                    newRoom.GetComponent<RoomScriptTEST>().xcoord = currentRoom[0].GetComponent<RoomScriptTEST>().xcoord + 1;
                    prevRoomDir = 'L';
                    break;
                default:
                    break;
                }

                for (int i = 0; i < currentRoom[0].GetComponent<RoomScriptTEST>().doors.Count; i++)
                {
                    Destroy(currentRoom[0].GetComponent<RoomScriptTEST>().doors[i]);
                }

                //deactivate blocker
                spaces[newRoom.GetComponent<RoomScriptTEST>().xcoord + 7, newRoom.GetComponent<RoomScriptTEST>().ycoord + 7].SetActive(false);

                //spaces[(int)position.x + 2, (int)position.y + 2] = newRoom;

                currentRoom[0].GetComponent<RoomScriptTEST>().doors.Clear();

                //current room becomes the new room
                for (int i = 0; i < currentRoom.Count; i++) {
                    prevRoom.Add(currentRoom[i]);
                }

                currentRoom.Clear();
                currentRoom.Add(newRoom);

                createEvent(position);

                //make some new rooms
                generateDoor(d.GetComponent<RoomGeneratingDoor>().roomDir);

                break;
            case 4:
                break;
            default:
                break;
        }

          

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

        switch (num)
        {
            case 0:
                GameObject Ammo = Instantiate(ammoCrate, position, Quaternion.identity);
                currentRoom[0].GetComponent<RoomScriptTEST>().eventItem = Ammo;
                break;
            case 1:
                GameObject Tele = Instantiate(Teleporter, position, Quaternion.identity);
                currentRoom[0].GetComponent<RoomScriptTEST>().eventItem = Tele;
                break;
            case 2:
                GameObject SpeedPack = Instantiate(SpeedBoost, position, Quaternion.identity);
                currentRoom[0].GetComponent<RoomScriptTEST>().eventItem = SpeedPack;
                break;
            case 3:
                GameObject monster = Instantiate(eventMonster, position, Quaternion.identity);
                currentRoom[0].GetComponent<RoomScriptTEST>().eventItem = monster;
                break;
            case 4:
                GameObject HPup = Instantiate(healthUp, position, Quaternion.identity);
                currentRoom[0].GetComponent<RoomScriptTEST>().eventItem = HPup;
                break;
            default:
                break;
        }
        
    }

    public void telport(int x, int y)
    {

        for (int i = 0; i < currentRoom.Count; i++)
        {
            spaces[currentRoom[i].GetComponent<RoomScriptTEST>().xcoord+7, currentRoom[i].GetComponent<RoomScriptTEST>().ycoord+7].SetActive(true);
            currentRoom[i].GetComponent<RoomScriptTEST>().DestroyMe();
        }
        currentRoom.Clear();

        //restart blockers and delete rooms
        if(prevRoom != null)
        {
            for (int i = 0; i < currentRoom.Count; i++)
            {
                spaces[prevRoom[i].GetComponent<RoomScriptTEST>().xcoord + 7, prevRoom[i].GetComponent<RoomScriptTEST>().ycoord + 7].SetActive(true);
                prevRoom[i].GetComponent<RoomScriptTEST>().DestroyMe();
            }

        }
        prevRoom.Clear();

        //destroy all currently active doors

        spaces[x, y].SetActive(false);

        Vector3 position = new Vector3(2*(x-7), 2*(y-7), 0);

        currentRoom.Add(Instantiate(room, position, Quaternion.identity));
        currentRoom[0].GetComponent<RoomScriptTEST>().xcoord = x-7;
        currentRoom[0].GetComponent<RoomScriptTEST>().ycoord = y-7;
        GameObject player = GameObject.Find("Player");
        player.transform.position = position;

        generateDoor('X');
    }

}
