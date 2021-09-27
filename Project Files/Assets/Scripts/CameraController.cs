using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject Target;
    private bool is_locked = false;


    void Update()
    {
        FollowTarget();//follows a target each frame
    }

    //Camera locking function - false locks the camera and let's the target walk around freely, true - lock the camera to the target's movement
    public void SetCameraLock(bool status)
    { 
        is_locked = status;
    }

    //set's the camera target
    public void SetTarget(GameObject target) 
    {
        if (target != null)
            Target = target;
    }

    //follows a target's movement each frame
    private void FollowTarget()
    {
        if (Target == null) return;
        if (is_locked) return;

        transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
    }
}
