using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public GameObject[] RoomPrefabs;
    public GameObject[] RoomScales;
    float rX = 0, rY = 0, rSX = 0, rSY = 0;
    int roomCount = 0, cGR = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in RoomPrefabs)
        {
            if(go.activeSelf)
            {
                roomCount++;
            }
        }

        Instantiate(RoomPrefabs[0], new Vector3(0, 0, 0), Quaternion.identity);
        for (var i = 0; i < 9; i++)
        {
            cGR = UnityEngine.Random.Range(1, roomCount-1);

            //rSX = transform.localScale.x;
            //rSY = transform.localScale.y;

            while(rX>-10.0f && rX<10.0f)
            {
                rX = UnityEngine.Random.Range(-30.0f, 30.0f) + i * UnityEngine.Random.Range(-20.0f, 20.0f) + UnityEngine.Random.Range(-rSX, rSX);
            }
            while(rY>-10.0f && rY<10.0f)
            {
                rY = UnityEngine.Random.Range(-30.0f, 30.0f) + i * UnityEngine.Random.Range(-20.0f, 20.0f) + UnityEngine.Random.Range(-rSY, rSY);
            }
            print("i = " + i + " rX = " + rX + " rY = " + rY);


            
            Instantiate(RoomPrefabs[cGR], new Vector3(rX, rY, 0), Quaternion.identity);
            rX = 0;
            rY = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
