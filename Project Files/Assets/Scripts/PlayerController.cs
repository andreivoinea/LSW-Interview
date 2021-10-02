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

    //Part of the stats window, hold the information about the player's gold
    public int Currency = 0;

    //If the player tries to interact with anything it sets the interaction status to true
    [HideInInspector]
    public bool interactStatus = false;

    public SpriteRenderer Hat;
    public SpriteRenderer Torso;
    public SpriteRenderer Legs;
    public SpriteRenderer Shoes;


    //Called before the first Start Methods, it is used to set the references 
    public void Awake()
    {
        //Focuses the main camera to the player
        mainCamera = Camera.main.GetComponent<CameraController>();
        FocusCamera(gameObject);
    }
    private void Start()
    {
        //Sets the Game Controller's player reference to the actual player
        GameController.Instance.SetPlayer(this);
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
        Interact();
        Quit();
    }

    //Method for the player's movement, it uses the player's movement speed and the user's input to move the player on the map
    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(inputX, inputY) * movementSpeed;
    }

    public bool canInteract = false;
    public bool isInteracting = false;

    //Method for the player's interaction ability
    public void Interact()
    {
        if (!canInteract) return;
        if (Input.GetKeyDown("e")) isInteracting = !isInteracting;
    }

    private void Inventory()
    {
        if (Input.GetKeyDown("i")) //Show the inventory from the Game Controllor's method
            GameController.Instance.ShowInventory(!GameController.Instance.inventoryStatus);
    }

    private void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    public void EquipItem(int equipmentType,Sprite equipmentSprite)
    {
        switch (equipmentType)
        {
            case 0:
                Hat.sprite= equipmentSprite;
                return;
            case 1:
                Torso.sprite = equipmentSprite;
                return;
            case 2:
                Legs.sprite = equipmentSprite;
                return;
            case 3:
                Shoes.sprite = equipmentSprite;
                return;
            default:
                return;

        }
    }

    public void UnequipItem(int equipmentType, Sprite equipmentSprite)
    {
        switch (equipmentType)
        {
            case 0:
                if(Hat.sprite == equipmentSprite) Hat.sprite = null;
                return;
            case 1:
                if (Torso.sprite == equipmentSprite) Torso.sprite = null;
                return;
            case 2:
                if (Legs.sprite == equipmentSprite) Legs.sprite = null;
                return;
            case 3:
                if (Shoes.sprite == equipmentSprite) Shoes.sprite = null;
                return;
            default:
                return;

        }

    }


}
