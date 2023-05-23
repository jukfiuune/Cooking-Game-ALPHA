using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenuButton : MonoBehaviour
{
    public GameObject CraftingWindow;
    CraftingWindow cw;
    OpenedUI UIOpen;
    // Start is called before the first frame update
    private void Start()
    {
        cw = CraftingWindow.GetComponent<CraftingWindow>();
        UIOpen = GameObject.Find("UIOpen").GetComponent<OpenedUI>();
    }
    public void OpenCloseWindow()
    {
        CraftingWindow.SetActive(!CraftingWindow.activeSelf);
        if (CraftingWindow.activeSelf)
        {
            cw.TurnOn();
        }
        else
        {
            UIOpen.openNonInvetoryUI = null;
        }
    }
}
