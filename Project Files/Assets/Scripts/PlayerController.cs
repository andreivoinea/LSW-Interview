using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //player's movement speed
    public float movementSpeed;

    //main camera controller
    private CameraController mainCamera;

    private GameController Controller;

    public int Currency = 0;

    //if the player tries to interact it sets the interaction status to true
    [HideInInspector]
    public bool interactStatus = false;

    public 

    void Awake()
    {
        //focuses the main camera to the player
        mainCamera = Camera.main.GetComponent<CameraController>();
        FocusCamera(gameObject);

        Controller = GameObject.Find("Game Controller").GetComponent<GameController>();

        Controller.SetPlayer(this);
    }

    //function that sets the camera's target and lock status
    public void FocusCamera(GameObject target = null, bool lockStatus = false)
    {
        if (mainCamera == null) return;

        if (target != null)
            mainCamera.SetTarget(target);
        mainCamera.SetCameraLock(lockStatus);

    }

    void Update()
    {
        Movement();
        Inventory();
    }

    //function for the player's movement
    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(inputX, inputY) * movementSpeed;
    }

    // function for the player's interaction ability
    public bool Interact()
    {
        return Input.GetKey("e");
    }

    private bool inventoryStatus = false;
    private void Inventory()
    {
        if (Input.GetKeyDown("i"))
        {
            inventoryStatus = !inventoryStatus;
        }

        Controller.ShowInventory(inventoryStatus);
    }


}
