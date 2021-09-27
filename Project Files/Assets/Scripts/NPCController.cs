using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    //variable that marks if the NPC has been interacted with
    protected bool interacted;

    //If the player is in range show the interact UI
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            ShowUI();
        }
    }

    //Check if the player is interacting with the NPC
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(col.gameObject.GetComponent<PlayerController>().interactStatus) interacted = true;
            Debug.Log("interacted " + interacted);
        }
    }

    virtual protected void ShowUI() { }



}
