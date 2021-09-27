using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player's movement speed
    public float movementSpeed = 0.02f;

    //main camera controller
    private CameraController mainCamera;

    //if the player tries to interact it sets the interaction status to true
    [HideInInspector]
    public bool interactStatus = false;

    void Start()
    {
        //focuses the main camera to the player
        mainCamera = Camera.main.GetComponent<CameraController>();
        FocusCamera(gameObject);
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
    }

    //function for the player's movement
    private void Movement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        transform.position += new Vector3(inputX, inputY, 0) * movementSpeed;
    }

    // function for the player's interaction ability
    public bool Interact()
    {
        return Input.GetKey("e");
    }


}
