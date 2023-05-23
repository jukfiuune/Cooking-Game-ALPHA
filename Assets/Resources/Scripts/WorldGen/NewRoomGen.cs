using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRoomGen : MonoBehaviour
{
    public List<GameObject> roomPrefabs;
    public List<GameObject> scalePrefabs;
    public int numRooms = 8;
    public float roomSpacing = 1.0f;
    public Transform startRoom;
    public bool useXAxis = true;

    private List<Transform> generatedRooms = new List<Transform>();

    private void Start()
    {
        // Add the starting room to the list of generated rooms
        generatedRooms.Add(startRoom);

        for (int i = 0; i < numRooms; i++)
        {
            // Select a random room prefab
            int rangePF = Random.Range(0, roomPrefabs.Count);
            GameObject roomPrefab = roomPrefabs[rangePF];
            GameObject scalePrefab = scalePrefabs[rangePF];
            // Select a random room to base the new room on
            int rangeB = Random.Range(0, generatedRooms.Count);
            Transform baseRoom = generatedRooms[rangeB];
            string baseRoomName = baseRoom.name;
            int baseRoomNum = 0;
            GameObject scaleRoom;
            for(int j = 0; j < roomPrefabs.Count; j++)
            {
                if(baseRoomName==roomPrefabs[j].name){
                    baseRoomNum = j;
                    scaleRoom = scalePrefabs[baseRoomNum];
                }
            }
            // Calculate the offset for the new room based on the size and scale of the rooms
            float offset = (scaleRoom.transform.localScale.x * scaleRoom.transform.localScale.x + scalePrefab.transform.localScale.x * roomPrefab.transform.localScale.x) * 0.5f + roomSpacing;
            // Select a random position
            Vector3 newPos = baseRoom.position + (useXAxis ? Vector3.right : Vector3.up) * offset;
            // Store the positions
            List<Vector2> generatedRoomPositions = new List<Vector2>();
            foreach (Transform room in generatedRooms)
            {
                generatedRoomPositions.Add(new Vector2(room.position.x, room.position.y));
            }
            // Check if the new position would collide with an existing room
            while (generatedRoomPositions.Contains(new Vector2(newPos.x, newPos.y)))
            {
                offset += (scaleRoom.transform.localScale.x * scaleRoom.transform.localScale.x + scalePrefab.transform.localScale.x * roomPrefab.transform.localScale.x) * 0.5f + roomSpacing;
                newPos = baseRoom.position + (useXAxis ? Vector3.right : Vector3.up) * offset;
            }
            // Convert the position to a Vector2
            Vector2 newPos2D = new Vector2(newPos.x, newPos.y);
            // Generate the new room
            GameObject newRoom = Instantiate(roomPrefab, newPos2D, Quaternion.identity);
            generatedRooms.Add(newRoom.transform);
        }
    }
}
