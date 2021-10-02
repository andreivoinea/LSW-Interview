using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPCController : MonoBehaviour
{
    //This Script is attached to NPCs, it contains methods for NPC interaction
    //
    //
    private bool interacted=false;
    //Method that shows the interact UI if the player is in range 
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerController>().canInteract = true;
            ShowUI();
        }
    }


    //Method that checks if the player is interacting with the NPC
    private void Update()
    {
        if(GameController.Instance.Player == null) return;
        OnInteracted(GameController.Instance.Player.isInteracting, out _);
    }

    //Method that fires when the player is out of range and can no longer interact with the NPC
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().canInteract = false;
            col.gameObject.GetComponent<PlayerController>().isInteracting = false;
            OnInteracted(false,out _);
        }
    }


    //Methods that shows the Interact UI
    abstract protected void ShowUI();

    //Method that fires when the player interacts or stops interracting with the NPC
    virtual protected void OnInteracted(bool interacting,out bool returnStatus) 
    {
        if (interacted == interacting) { returnStatus = true; return; }

        if (interacting) GameController.Instance.ShowInventory(false);
        GameController.Instance.PlayerInteracting(interacting);

        interacted = interacting;

        returnStatus = false;
    }





}
