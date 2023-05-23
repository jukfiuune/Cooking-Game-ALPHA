using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCPlayerMovement : MonoBehaviour
{
    

    public Damage d;

    public Inventory MouseInventory;

    public float playerSpeed;
    private Rigidbody2D rb;
    private Vector2 playerDirection;
    public float playerSpeedBuff;

    public int hunger = 200;
    public int maxHunger = 200;

    public Inventory playerInventory;

    SpriteRenderer toolSprite;

    SpriteRenderer armorSprite;

    SpriteRenderer helmetSprite;

    public Vector3 PointToMoveTo;
    public bool ShouldMoveToPoint = false;
    public InventoryInteract inventoryToOpen;
    public string moveToPointAction;
    public float moveToRange;

    public bool ItemUseMousePositionInstead;
    // Start is called before the first frame update
    void Start()
    {
        MouseInventory = GameObject.Find("PlayerInventory").GetComponent<DisplayInventory>().MouseInventory;

        d = GameObject.Find("Healthbar").GetComponent<Damage>();

        rb = GetComponent<Rigidbody2D>();

        playerInventory = GetComponent<Inventory>();

        toolSprite = GameObject.Find("Tool").GetComponent<SpriteRenderer>();

        armorSprite = GameObject.Find("Armor").GetComponent<SpriteRenderer>();

        helmetSprite = GameObject.Find("Helmet").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPoint(PointToMoveTo, moveToPointAction);

        if(Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("w") || Input.GetKey("s")){
            ShouldMoveToPoint = false;
            GameObject bP = GameObject.Find("BuildingPlacer");
            if (bP != null) {
                BuildingPlacerScript buildingPlacerScript = bP.GetComponent<BuildingPlacerScript>();
                if (buildingPlacerScript != null)
                {
                    buildingPlacerScript.lockPosition = false;
                }
            }
        }  
        float dX = Input.GetAxisRaw("Horizontal");
        float dY = Input.GetAxisRaw("Vertical");

        playerDirection = new Vector2(dX,dY).normalized;

        if(playerInventory.items[9] != null){
            DisplayToolSprite();
        }else{
            toolSprite.sprite = null;
        }

        if(playerInventory.items[10] != null){
            DisplayArmorSprite();
        }else{
            armorSprite.sprite = null;
        }

        if(playerInventory.items[11] != null){
            DisplayHelmetSprite();
        }else{
            helmetSprite.sprite = null;
        }

        if(Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if(MouseInventory.items[0] != null){

                if(MouseInventory.items[0].GetMisc() != null){
                    ItemUseMousePositionInstead = true;
                    MouseInventory.items[0].UseItem(this);
                }else{
                    DropItemFromPlayer();
                }

            }
            else if (playerInventory.items[9] != null){
                if(playerInventory.items[9].GetTool().CanSwing()){
                    ItemUseMousePositionInstead = true;
                    playerInventory.items[9].UseItem(this);
                }
            }
        }    
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(playerDirection.x*playerSpeed*playerSpeedBuff,playerDirection.y*playerSpeed*playerSpeedBuff);
    }

    public void DisplayToolSprite(){
        toolSprite.sprite = playerInventory.items[9].itemIcon;
    }

    public void DisplayArmorSprite(){
        armorSprite.sprite = playerInventory.items[10].itemIcon;
    }

    public void DisplayHelmetSprite(){
        helmetSprite.sprite = playerInventory.items[11].itemIcon;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {    
        if (collision.gameObject.tag == "projectile")
        {
            d.DealDamage(5);
                
            Destroy(GameObject.FindGameObjectWithTag("projectile"));
        }
    }

    public void MoveToPoint(Vector3 MoveTo, string action){
        MoveTo = new Vector3(MoveTo.x, MoveTo.y, transform.position.z);
        if (ShouldMoveToPoint){
            if(Vector3.Distance(transform.position, MoveTo) > moveToRange){
                float step = playerSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, MoveTo, step);
            }else{
                ShouldMoveToPoint = false;
                if (action == "OpenInventory")
                {
                    inventoryToOpen.OpenInventory();
                }
                else if (action == "UseItem")
                {
                    ItemUseMousePositionInstead = false;
                    playerInventory.items[9].UseItem(this);
                }
                else if (action == "UseHand")
                {
                    ItemUseMousePositionInstead = false;
                    MouseInventory.items[0].UseItem(this);
                }
                else if (action == "PlaceBuilding")
                {
                    GameObject bP = GameObject.Find("BuildingPlacer");
                    if (bP != null)
                    {
                        BuildingPlacerScript buildingPlacerScript = bP.GetComponent<BuildingPlacerScript>();
                        if (buildingPlacerScript != null)
                        {
                            buildingPlacerScript.PlaceBuilding(PointToMoveTo);
                        }
                    }
                }
            }
        }
    }

    public void DropItemFromPlayer(){
        Vector3 positionToDrop = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionToDrop = new Vector3(positionToDrop.x, positionToDrop.y, 0);
        MouseInventory.items[0].DropItem(positionToDrop, MouseInventory.items[0], MouseInventory.amounts[0]);
        MouseInventory.RemoveAt(0, MouseInventory.amounts[0]);
    }
}
