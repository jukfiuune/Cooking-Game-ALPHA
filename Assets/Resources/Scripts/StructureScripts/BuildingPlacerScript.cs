using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacerScript : MonoBehaviour
{
    public BuildingClass building;
    bool canBePlaced = true;
    public Color placeableColor;
    public Color blockedColor;
    bool canBePlacedThisFrame = true;
    SpriteRenderer sr;
    SCPlayerMovement player;
    public bool lockPosition = false;
    private void Start()
    {
        transform.name = "BuildingPlacer";
        sr = GetComponent<SpriteRenderer>();
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        BoxCollider2D builingCollider = building.building.GetComponent<BoxCollider2D>();
        collider.size = builingCollider.size;
        collider.offset = builingCollider.offset;
        player = GameObject.Find("Body").GetComponent<SCPlayerMovement>();
    }
    void LateUpdate()
    {
        if (!lockPosition)
        {
            Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mp.x, mp.y, transform.position.z);
        }

        canBePlacedThisFrame = true;
        if (canBePlaced && Input.GetMouseButton(0))
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 3)
            {
                PlaceBuilding(transform.position);
            }
            else
            {
                player.PointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                player.ShouldMoveToPoint = true;
                player.moveToPointAction = "PlaceBuilding";
                player.moveToRange = 3;
                lockPosition = true;
            }
        }
    }
    public void PlaceBuilding(Vector3 location)
    {
        location = new Vector3(location.x, location.y, 0);
        Instantiate(building.building, location, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RemovePlaceable();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        RemovePlaceable();
    }
    void RemovePlaceable()
    {
        canBePlaced = false;
        sr.color = blockedColor;
        canBePlacedThisFrame = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (canBePlacedThisFrame)
        {
            canBePlaced = true;
            sr.color = placeableColor;
        }

    }


}
