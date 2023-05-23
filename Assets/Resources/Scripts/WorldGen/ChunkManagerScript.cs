using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManagerScript : MonoBehaviour
{
    public int chunkMapSize;
    int offsetFrom0;
    int chunkSize = 10;
    GameObject[,] chunkMap;
    GameObject chunkPrefab;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        offsetFrom0 = chunkMapSize/2;
        chunkMap = new GameObject[chunkMapSize, chunkMapSize];
        chunkPrefab = Resources.Load<GameObject>("Prefabs/Chunk");
        player = GameObject.Find("Body");
        for (int x = 0; x < chunkMapSize; x++)
        {
            for (int y = 0; y < chunkMapSize; y++)
            {
                Vector3 pos = new Vector3((x-offsetFrom0) * chunkSize, (y - offsetFrom0) * chunkSize, 0);
                GameObject chunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
                chunk.GetComponent<ChunkScript>().player = player;
                chunkMap[x, y] = chunk;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int xInGrid = Mathf.FloorToInt((player.transform.position.x + offsetFrom0* chunkSize) / chunkSize);
        int yInGrid = Mathf.FloorToInt((player.transform.position.y + offsetFrom0 * chunkSize) / chunkSize);
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x + xInGrid >= 0 && y + yInGrid >= 0 && x + xInGrid < chunkMapSize && y + yInGrid < chunkMapSize)
                {
                    chunkMap[x + xInGrid, y + yInGrid].SetActive(true);
                }
            }
        }
    }
}
