using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpotScript : MonoBehaviour
{
        
    public FarmPlotScript myFarmPlot;
        
    public PlantBase Plant;
    SpriteRenderer image;

    bool Initiated = false;

    float TimePerStage;
    float CheckTime;
    public float timerTPS;
    float timerCT;
    int Stages;
    int CurrentStage = 0;

    int needWater;
    int needFertilizer;

    bool ready = false;

    public GameObject itemPickupPrefab; 
    void Start()
    {
        image = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        // Sort By Y
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -32);
    }

    // Update is called once per frame
    void Update()
    {
        if (Plant != null)
        {
            if (!Initiated)
            {
                // Get the values out of PlantBase Plant
                Initiated = true;

                Stages = Plant.Stages.Length;
                TimePerStage = Plant.baseTimeToGrow / Stages;
                CheckTime = TimePerStage / 2;
                timerTPS = TimePerStage;
                timerCT = CheckTime;

                needWater = Plant.waterUnitsNeeded;
                needFertilizer = Plant.fertilizerUnitsNeeded;
                image.sprite = Plant.Stages[0];
            }
            if (CurrentStage != Stages - 1)
            {
                //Check Needs Timer
                if (timerCT <= 0)
                {
                    CheckNeeds(); // Check for the plants needs

                    timerCT = CheckTime; // Reset check timer
                }
                else
                {
                    timerCT -= Time.deltaTime; // TImer
                }

                //Grow Timer
                if (timerTPS <= 0)
                {
                    // Update the plant stage
                    CurrentStage++;
                    image.sprite = Plant.Stages[CurrentStage];

                    timerTPS = TimePerStage; // Reset grow time
                }
                else
                {
                    timerTPS -= Time.deltaTime; // Timer
                }
            }
            else
            {
                // Crop has finished growing
                ready = true;
            }
        }
    }

    private void OnMouseDown()
    {
        // Harvest
        if (ready)
        {
            for (int i = 0; i < Plant.products.Length; i++) {
                itemPickup item = Instantiate(itemPickupPrefab, this.transform.position, Quaternion.identity).GetComponent<itemPickup>();
                item.item = Plant.products[i];
                item.amount = Plant.amounts[i];
            }
            for (int i = 0; i < myFarmPlot.soilList.Count; i++)
            {
                if (myFarmPlot.soilList[i] == gameObject)
                {
                    myFarmPlot.soilList.RemoveAt(i);
                    break;
                }
            }
            Destroy(gameObject);
        }

    }
    void CheckNeeds()
    {
        // Check the needs of the plant
        if (needWater != 0)
        {
            if (myFarmPlot.waterLevel > 0)
            {
                needWater--;
                myFarmPlot.waterLevel--;
            }
            else
            {
                // Not enough water
                SlowDownGrowth();
            }
        }
        if (needFertilizer != 0)
        {
            if (myFarmPlot.fertilizerLevel > 0)
            {
                needFertilizer--;
                myFarmPlot.fertilizerLevel--;
            }
            else
            {
                // Not enough fertilizer
                SlowDownGrowth();
            }
        }
        myFarmPlot.UpdateSprites();
    }
    void SlowDownGrowth()
    {
        timerTPS += timerTPS / 5;
    }
}

