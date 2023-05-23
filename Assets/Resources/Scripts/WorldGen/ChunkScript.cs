using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkScript : MonoBehaviour
{
    public GameObject player;
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 30f)
        {
            gameObject.SetActive(false);
        }
    }
}
