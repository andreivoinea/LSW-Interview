using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //This Script can be attached to cameras, it contains methods for locking the position and following the player
    //
    //

    //Target Componenet for the Follow method
    private GameObject Target;

    //Lock Variable for locking the camera movement
    private bool is_locked = false;


    void Update()
    {
        FollowTarget();//Follows a target each frame
    }

    //Camera locking method - false locks the camera and let's the target walk around freely, true - lock the camera to the target's movement
    public void SetCameraLock(bool status)
    { 
        is_locked = status;
    }

    //Method that set's the camera target to a specified object
    public void SetTarget(GameObject target) 
    {
        if (target != null)
            Target = target;
    }

    //Method that follows a target's movement each frame
    private void FollowTarget()
    {
        if (Target == null) return;
        if (is_locked) return;

        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
    }
}
