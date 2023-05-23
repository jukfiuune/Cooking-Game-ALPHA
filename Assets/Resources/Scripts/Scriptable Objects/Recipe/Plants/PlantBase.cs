using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Plant")]
public class PlantBase : ScriptableObject
{
    [Header("Plant")]
    [Header("Products:")]
    public Item[] products;
    public int[] amounts;

    public Sprite[] Stages;
    [Header("Total amount of time/needs:")]
    // not divided by stages
    public float baseTimeToGrow;
    public int waterUnitsNeeded;
    public int fertilizerUnitsNeeded;
}
