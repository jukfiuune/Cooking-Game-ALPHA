using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CraftingWindow : MonoBehaviour
{
    public GameObject[] buttons = new GameObject[8];
    bool[] craftableAt;
    public GameObject[] pageTurnButtons = new GameObject[2];
    Inventory playerInventory;
    GameObject rightPage;
    CraftingRecipes[] recipes;
    CraftingRecipes[] filteredRecipes;
    public int page = 0;
    public int maxPage;
    int indexSelected = 0;
    bool firstStart = true;

    OpenedUI UIOpen;
    private void Start() 
    {
        UIOpen = GameObject.Find("UIOpen").GetComponent<OpenedUI>();

        craftableAt = new bool[buttons.Length];
        rightPage = GameObject.Find("RightPage");
        rightPage.SetActive(false);
        recipes = Resources.LoadAll<CraftingRecipes>("Recipes/Crafting");

        playerInventory = GameObject.Find("Body").GetComponent<Inventory>();

        firstStart = false;
        TurnOn();
    }
    public void TurnOn()
    {
       
        if (!firstStart)
        {
            if (UIOpen.openedInventory != null)
            {
                UIOpen.openedInventory.GetComponent<InventoryInteract>().CloseInventory();
            }
            UIOpen.openNonInvetoryUI = gameObject;

            rightPage.SetActive(false);
            Filter("All");
        }
    }
    public void Filter(string _filter)
    {
        filteredRecipes = new CraftingRecipes[recipes.Length];
        int count = 0;
        if (_filter == "All")
        {
            filteredRecipes = recipes;
            count = filteredRecipes.Length;
        }
        else
        {
            for (int i = 0; i < recipes.Length; i++)
            {
                if (recipes[i].category == _filter)
                {
                    filteredRecipes[count] = recipes[i];
                    count++;
                }
            }
        }
        page = 0;
        maxPage = Mathf.CeilToInt((float)count / buttons.Length);
        rightPage.SetActive(false);
        MovePageBy(0); // I am using MovePageBy() instead of OnPageUpdate(), because i need to
        //             // update the page turn buttons and MovePageBy() contains OnPageUpdate()
    }
    public void MovePageBy(int _page)
    {
        if (page + _page >= 0 && page + _page + 1 <= maxPage)
        {
            page += _page;
            OnPageUpdate();
        }
        if (page == 0) pageTurnButtons[0].SetActive(false);
        else pageTurnButtons[0].SetActive(true);
        if (page+1 == maxPage) pageTurnButtons[1].SetActive(false);
        else pageTurnButtons[1].SetActive(true);
    }
    public void OnPageUpdate()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Image _image = buttons[i].GetComponent<Image>();
            TextMeshProUGUI _text = _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            int indexOfProduct = page * buttons.Length + i;
            if (indexOfProduct < filteredRecipes.Length && filteredRecipes[indexOfProduct] != null)
            {
                buttons[i].GetComponent<Button>().interactable = true;

                Item product = filteredRecipes[indexOfProduct].product;
                _image.sprite = product.itemIcon;
                
                if (checkIfCraftable(filteredRecipes[indexOfProduct]))
                {
                    craftableAt[i] = true;
                    _image.color = new Color(1, 1, 1, 1);
                }
                else
                {
                    craftableAt[i] = false;
                    _image.color = new Color(0.8f, 0.1f, 0.1f, 0.8f);
                }

                _text.text = product.Name;
            }
            else
            {
                buttons[i].GetComponent<Button>().interactable = false;

                _image.sprite = null;
                _image.color = new Color(0, 0, 0, 0);

                _text.text = "";
            }
        }
    }
    public void UpdateSelection(int selectionButtonIndex)
    {
        indexSelected = page * buttons.Length + selectionButtonIndex;

        CraftingRecipes currentRecipe = filteredRecipes[indexSelected];

        rightPage.SetActive(true);

        rightPage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentRecipe.product.Name;

        Image bigImage = rightPage.transform.GetChild(1).GetComponent<Image>();
        bigImage.sprite = currentRecipe.product.itemIcon;
        bigImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentRecipe.amount.ToString();


        rightPage.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = currentRecipe.product.Description;

        for (int i = 0; i < currentRecipe.Requirements.Length; i++)
        {
            Image _image = rightPage.transform.GetChild(4 + i).GetComponent<Image>();
            _image.sprite = currentRecipe.Requirements[i].itemIcon;
            _image.color = new Color(1, 1, 1, 1);

            _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().GetComponent<TextMeshProUGUI>().text = currentRecipe.RequirementsAmount[i].ToString();
        }
        for (int i = 2; i >= currentRecipe.Requirements.Length; i--)
        {
            Image _image = rightPage.transform.GetChild(4 + i).GetComponent<Image>();
            _image.sprite = null;
            _image.color = new Color(0, 0, 0, 0);

            _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().GetComponent<TextMeshProUGUI>().text = "";

        }

        Image buttonImage = rightPage.transform.GetChild(3).GetComponent<Image>();
        if (craftableAt[selectionButtonIndex])
        {
            bigImage.color = new Color(1, 1, 1, 1);
            buttonImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            bigImage.color = new Color(0.8f, 0.1f, 0.1f, 1);
            buttonImage.color = new Color(0.8f, 0.1f, 0.1f, 1);
        }
    }
    public bool checkIfCraftable(CraftingRecipes recipe)
    {
        for (int i = 0; i < recipe.Requirements.Length; i++)
        {
            if (playerInventory.GetItemAmount(recipe.Requirements[i]) < recipe.RequirementsAmount[i])
            {
                return false;
            }
        }
        return true;
    }
    public void Craft()
    {
        CraftingRecipes craftingRecipes = filteredRecipes[indexSelected];
        if (craftableAt[indexSelected%8])
        {
            // Normal crafting
            if (craftingRecipes.product.GetBuilding() == null)
            {
                // Remove items from the players inventory
                for (int i = 0; i < craftingRecipes.Requirements.Length; i++)
                {
                    playerInventory.RemoveItem(craftingRecipes.Requirements[i], craftingRecipes.RequirementsAmount[i]);
                }

                // Re add them if there isn't enough space for the product.
                // If we remove the items from the players inventory after we make sure they can fit 
                // we miss out on a special case that should allow the crafting of an item (if the player has a full inventory
                // but crafting an item would free up a slot that that item should be crafted)
                if (playerInventory.GetItemFitAmount(craftingRecipes.product) >= craftingRecipes.amount)
                {
                    playerInventory.AddItem(craftingRecipes.product, craftingRecipes.amount);

                }
                else
                {
                    // Adding the product to the players inventory
                    for (int i = 0; i < craftingRecipes.Requirements.Length; i++)
                    {
                        playerInventory.AddItem(craftingRecipes.Requirements[i], craftingRecipes.RequirementsAmount[i]);
                        // End
                    }
                }
            }
            //Crafting a building
            else
            {
                if (GameObject.Find("BuildingPlacer") == null)
                {
                    // Remove building cost from the players inventory
                    for (int i = 0; i < craftingRecipes.Requirements.Length; i++)
                    {
                        playerInventory.RemoveItem(craftingRecipes.Requirements[i], craftingRecipes.RequirementsAmount[i]);
                    }

                    GameObject bp = Instantiate(Resources.Load<GameObject>("Prefabs/Structures/BuildingPlacer"), transform.position, Quaternion.identity);
                    bp.GetComponent<BuildingPlacerScript>().building = craftingRecipes.product.GetBuilding();
                    bp.GetComponent<SpriteRenderer>().sprite = craftingRecipes.product.itemIcon;
                    gameObject.SetActive(false);
                    // End
                }
            }
        }
    }
}
