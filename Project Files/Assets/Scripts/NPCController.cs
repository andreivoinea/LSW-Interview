using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    //This Script is attached to NPCs, it contains methods for NPC interaction
    //
    //

    //Variable that marks if the NPC has been interacted with
    protected bool interacted;

    //Method that shows the interact UI if the player is in range 
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            ShowUI();
        }
    }
    //Method that checks if the player is interacting with the NPC
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(col.gameObject.GetComponent<PlayerController>().Interact()) interacted = true;
            Debug.Log("interacted " + interacted);
        }
    }

    //Method that fires when the player is out of range and can no longer interact with the NPC
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            interacted = false;
        }
    }

    //Methods that shows the Interact UI
    virtual protected void ShowUI() { }



}
