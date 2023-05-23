using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructablesGeneration : MonoBehaviour
{
    GameObject[] ObjectsToSpawn;

    public int SpawnRadius;

    public int MinSpawnAmount;
    public int MaxSpawnAmount;  

    private void Start() {
        int AmountToSpawn = Random.Range(MinSpawnAmount, MaxSpawnAmount);

        ObjectsToSpawn = Resources.LoadAll<GameObject>("Prefabs/World Objects/Destructable Objects");

        for(int i = 0; i < AmountToSpawn; i++){
            Instantiate(ObjectsToSpawn[Random.Range(0, ObjectsToSpawn.Length)], new Vector2(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
        }
    }
}
