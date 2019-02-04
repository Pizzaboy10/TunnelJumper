using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public List<GameObject> rooms;
    public GameObject wallHolder;
    public int roomsToSpawn;
    [SerializeField] private Vector2 nextSpawnPoint;
    public GameObject oneWayPlatform;
    [SerializeField] private GameObject previousHeightMarker, recordHeightMarker;

    public bool limitedPoolTest;
    public List<GameObject> limitedPoolRooms;

    void Start()
    {
        for (int i = 0; i < roomsToSpawn; i++)
        {
            nextSpawnPoint = new Vector2(-10, 6 + 20 * i);
            if (limitedPoolTest)
            {
                int whichRoom = Random.Range(0, limitedPoolRooms.Count);
                GameObject room = Instantiate(limitedPoolRooms[whichRoom], nextSpawnPoint, Quaternion.identity);
                if (Random.value <= 0.5f)
                {
                    room.transform.localScale = new Vector3(-1, 1, 1);     //half of the time, flip the room. Technically doubled the room count now.
                }
            }
            else
            {
                int whichRoom = Random.Range(0, rooms.Count);
                GameObject room = Instantiate(rooms[whichRoom], nextSpawnPoint, Quaternion.identity);
                if (Random.value <= 0.5f)
                {
                    room.transform.localScale = new Vector3(-1, 1, 1);     //half of the time, flip the room. Technically doubled the room count now.
                }
            }


            Instantiate(wallHolder, nextSpawnPoint, Quaternion.identity);
            Instantiate(oneWayPlatform, nextSpawnPoint + new Vector2(0, 10), Quaternion.identity);

        }

        previousHeightMarker.transform.position = new Vector2(-10, PlayerPrefs.GetFloat("PreviousHeight", -20));
        recordHeightMarker.transform.position = new Vector2(-10, PlayerPrefs.GetFloat("RecordHeight", -20));
    }

    public void NewRoomGeneration()                                 //This function is called once the player gets high enough to beat the original generation.
    {
        nextSpawnPoint += new Vector2(0, 20);                       //Just add 20 to the next room height.

        int whichRoom = Random.Range(0, rooms.Count);
        GameObject room = Instantiate(rooms[whichRoom], nextSpawnPoint, Quaternion.identity);
        if (Random.value <= 0.5f)
        {
            room.transform.localScale = new Vector3(-1, 1, 1);     //half of the time, flip the room. Technically doubled the room count now.
        }
        Instantiate(wallHolder, nextSpawnPoint, Quaternion.identity);
        Instantiate(oneWayPlatform, nextSpawnPoint + new Vector2(0, 10), Quaternion.identity);
    }


}
