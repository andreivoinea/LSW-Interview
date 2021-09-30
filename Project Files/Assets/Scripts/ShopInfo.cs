using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInfo : NPCController
{
    //This Script is attached to a specific type of NPCs, mainly Shopkeepers. It Contains Methods for the Shopkeeper's inventory as well as it's UI
    //
    //

    //Inherited method that shows the interact UI
    override protected void ShowUI() 
    {
        Debug.Log("ShowUI");
    }
}
