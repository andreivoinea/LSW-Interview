using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //This Script is attached to the player, it contains all information related to the player from movent and interaction methods to player stats
    //
    //


    //player's movement speed
    public float movementSpeed;

    //Main Camera Controller, this is the Camera that renders the main game's screen
    private CameraController mainCamera;

    //The Game Controller is a central point to controlling most functionability of the game
    private GameController Controller;

    //Part of the stats window, hold the information about the player's gold
    public int Currency = 0;

    //If the player tries to interact with anything it sets the interaction status to true
    [HideInInspector]
    public bool interactStatus = false;

    //Called before the first Start Methods, it is used to set the references 
    public void Awake()
    {
        //Focuses the main camera to the player
        mainCamera = Camera.main.GetComponent<CameraController>();
        FocusCamera(gameObject);

        //Sets the Reference for the Game Controller
        Controller = GameObject.Find("Game Controller").GetComponent<GameController>();

        //Sets the Game Controller's player reference to the actual player
        Controller.SetPlayer(this);
    }

    //Method that sets the camera's target and lock status
    public void FocusCamera(GameObject target = null, bool lockStatus = false)
    {
        if (mainCamera == null) return;

        if (target != null)
            mainCamera.SetTarget(target);
        mainCamera.SetCameraLock(lockStatus);

    }

    //The Update method functions like a player's brain
    void Update()
    {
        Movement();
        Inventory();
    }

    //Method for the player's movement, it uses the player's movement speed and the user's input to move the player on the map
    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(inputX, inputY) * movementSpeed;
    }

    //Method for the player's interaction ability
    public bool Interact()
    {
        return Input.GetKey("e");
    }

    //Variable that hold information about the Player's action to open or close the Inventory Tab
    private bool inventoryStatus = false;
    private void Inventory()
    {
        if (Input.GetKeyDown("i"))
        {
            inventoryStatus = !inventoryStatus;
        }

        //Show the inventory from the Game Controllor's method
        Controller.ShowInventory(inventoryStatus);
    }


}
