using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmPlotScript : MonoBehaviour
{
    public int waterLevel = 0;
    public int fertilizerLevel = 0;
    public List<GameObject> soilList = new List<GameObject>();

    public SpriteRenderer fertilizerOverlay;
    public SpriteRenderer waterOverlay;

    private void Start()
    {
        fertilizerOverlay = transform.GetChild(0).GetComponent<SpriteRenderer>();
        waterOverlay = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    public void GiveNutrients(int waterUnits, int fertilizerUnits)
    {
        waterLevel += waterUnits;
        fertilizerLevel += fertilizerUnits;
        UpdateSprites();
    }
    public void UpdateSprites()
    {
        fertilizerOverlay.color = new Color(1, 1, 1, fertilizerLevel / 10f);
        waterOverlay.color = new Color(1, 1, 1, waterLevel / 10f);
    }
}
