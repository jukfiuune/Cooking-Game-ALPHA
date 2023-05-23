using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryPrefab;
    public float openDist;
    GameObject invObject;
    Inventory inv;
    GameObject player;
    SCPlayerMovement playerController;
    OpenedUI UIOpen;
    float dist;
    public bool opened = false;
    public bool interactable = true;
    void Start()
    {
        inv = GetComponent<Inventory>();
        player = GameObject.Find("Body");
        playerController = player.GetComponent<SCPlayerMovement>();
        UIOpen = GameObject.Find("UIOpen").GetComponent<OpenedUI>();
    }

    private void Update()
    {
        dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > openDist)
        {
            CloseInventory();
        }
    }

    private void OnMouseDown() {
        if(opened && !EventSystem.current.IsPointerOverGameObject()){
            CloseInventory();
        }
        else if (interactable)
        {
            OpenInventory();
        }
    }

    public void OpenInventory(){
        
        if (dist <= openDist)
        {
            if (UIOpen.openedInventory == null && UIOpen.openNonInvetoryUI == null)
            {
                CreateUI();
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            {
                if (UIOpen.openedInventory != null)
                {
                    UIOpen.openedInventory.GetComponent<InventoryInteract>().CloseInventory();
                }
                else
                {
                    UIOpen.openNonInvetoryUI.SetActive(false);
                }

                CreateUI();
            }
        }
        else if (!EventSystem.current.IsPointerOverGameObject())
        {
            playerController.PointToMoveTo = transform.position;
            playerController.ShouldMoveToPoint = true;
            playerController.inventoryToOpen = this;
            playerController.moveToPointAction = "OpenInventory";
            playerController.moveToRange = openDist;
        }
    }

    public void CreateUI()
    {
        invObject = Instantiate(inventoryPrefab);
        invObject.transform.SetParent(GameObject.Find("Canvas").transform);
        invObject.GetComponent<DisplayInventory>().TargetInventory = inv;
        invObject.transform.position = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0);
        UIOpen.openedInventory = inv;
        inv.InventoryUpdate();
        opened = true;
    }

    public void CloseInventory(){
        if(opened){
            Destroy(invObject);
            invObject = null;
            opened = false;
            UIOpen.openedInventory = null;
        }
    }
}
