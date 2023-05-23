using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationScript : MonoBehaviour
{
    // Start is called before the first frame update
    int chunkMapSize;
    int offsetFrom0;
    ChunkState[,] chunkMap;

    [Header("Generation Settings")]
    public int nodeAmount;
    public int minimumNodeInvalidDistance;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            chunkMapSize = GetComponent<ChunkManagerScript>().chunkMapSize;
            offsetFrom0 = chunkMapSize / 2;
            chunkMap = new ChunkState[chunkMapSize, chunkMapSize];
            for (int x = 0; x < chunkMapSize; x++)
            {
                for (int y = 0; y < chunkMapSize; y++)
                {
                    chunkMap[x, y] = new ChunkState();
                    chunkMap[x, y].x = x;
                    chunkMap[x, y].y = y;
                }
            }
            chunkMap[offsetFrom0, offsetFrom0].Stage1Type = "  N  ";
            Stage1();
            Stage2();
            Stage3();
            Stage4();
        }
    }

    void Stage1()
    {
        // * ------------- * Step 1: Create new nodes * ------------- * //
        int x, y, nodes = 0;
        ChunkState[] nodeStates = new ChunkState[nodeAmount];
        do
        {
            x = Random.Range(0, chunkMapSize);
            y = Random.Range(0, chunkMapSize);
            if (Stage1CheckSquare(minimumNodeInvalidDistance * 2+1, x, y))
            {
                chunkMap[x, y].Stage1Type = "  N  ";
                Debug.Log(nodes);
                nodeStates[nodes] = chunkMap[x, y];
                nodes++;
            }
        } while (nodes < nodeAmount);

        // * ------------- * Step 2: Expand Nodes * ------------- * //
        for (int i = 0; i < nodeStates.Length; i++)
        {
            ChunkState node = nodeStates[i];
            int budget = 50;
            int fails = 0;
            int totalFails = 0;
            List<ChunkState> expandChunks = new List<ChunkState>();
            while (budget > 0)
            {
                do
                {
                    x = Random.Range(-1, 2);
                    y = Random.Range(-1, 2);
                } while (!(x == 0 || y == 0));

                if (x + node.x > 0 && x + node.x < chunkMapSize && y + node.y > 0 && y + node.y < chunkMapSize)
                {
                    
                    if (chunkMap[x + node.x, y + node.y].Stage1Type == null)
                    {
                        // Try to buy node
                        int nodePrice = Mathf.Abs(x - offsetFrom0) + Mathf.Abs(y - offsetFrom0);
                        if (budget >= nodePrice)
                        {
                            chunkMap[node.x + x, node.y + y].Stage1Type = "  E  ";
                            expandChunks.Add(chunkMap[node.x + x, node.y + y]);
                            budget -= nodePrice;
                        }
                    }
                    else
                    {
                        fails++;
                        totalFails++;
                    }     
                }
                else
                {

                    fails++;
                    totalFails++;
                }
                if (fails > 5)
                {
                    // Change the target node
                    if (expandChunks.Count != 0)
                    {
                        node = expandChunks[Random.Range(0, expandChunks.Count)];
                    }
                    else
                    {
                        break;
                    }
                    fails = 0;

                }
                if (totalFails >= 100)
                {
                    // If the program has failed too many times it skips the current node and its expanded nodes
                    break;
                }
            }
        }
        DebugChunkMap(1);
        // * ------------- * Step 3: Connect Starter Nodes * ------------- * //

        // I hate this.

    }
    void Stage2()
    {

    }
    void Stage3()
    {

    }
    void Stage4()
    {

    }

    bool Stage1CheckSquare(int width, int xPos, int yPos)
    {
        // return true if all the chunks around a point have Stage1Type == null
        int bigHalf = width / 2;
        int smallHalf = width - bigHalf;
        for (int x = smallHalf; x < bigHalf; x++)
        {
            for (int y = smallHalf; y < bigHalf; y++)
            {
                if (x + xPos > 0 && x+xPos < chunkMapSize && y + yPos > 0 && y + yPos < chunkMapSize && chunkMap[x + xPos, y + yPos].Stage1Type != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
    void DebugChunkMap(int stage)
    {
        for (int x = 0; x < chunkMapSize; x++)
        {
            string line = "";
            for (int y = 0; y < chunkMapSize; y++)
            {
                if (stage == 1 && chunkMap[x, y].Stage1Type != null)
                {
                    line += chunkMap[x, y].Stage1Type;
                }
                else if (stage == 2 && chunkMap[x, y].Stage2Biome != null)
                {
                    line += chunkMap[x, y].Stage2Biome;
                }
                else if (chunkMap[x, y].Stage3Focus != null)
                {
                    line += chunkMap[x, y].Stage3Focus;
                }
                else
                {
                    line += "  .  ";
                }
            }
            Debug.Log(line);
        }
    }
    class ChunkState
    {
        public int x;
        public int y;
        public string Stage1Type;
        public string Stage2Biome;
        public string Stage3Focus;
    }
}
