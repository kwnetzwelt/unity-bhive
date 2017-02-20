using System;
using System.Collections.Generic;
using UnityEngine;

public class AIWalker : MonoBehaviour
{
    [SerializeField]
    Vector3 currentTarget;

	

    public float walkSpeed = 16;
    public float runSpeed = 24;
    public float gravity = 20.0f;
    public bool IsGrounded
    {
        get
        {
            return grounded;
        }
    }



    Vector3 contactPoint;
    CharacterController controller;
    Transform myTransform;
    bool run;
    private bool grounded = false;
    float speed;
    private RaycastHit hit;
    private float rayDistance;
    private float slideLimit;
    private bool falling;
    private float fallStartLevel;
    private float fallingDamageThreshold = 10;
    private float moveRotation = 0;
    private Vector3 moveDirection = Vector3.zero;


    float lastDistance;
    float currentDistance;
    private bool walk;
    public void WalkTo(Vector3 pPosition, bool pRun)
    {
        currentTarget = pPosition;
        run = pRun;
        walk = true;
        lastDistance = (currentTarget - transform.position).sqrMagnitude;

    }

    void OnEnable()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;


        speed = walkSpeed;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;

    }


    void Update()
    {

        // If the run button is set to toggle, then switch between walk/run speed. (We use Update for this...
        // FixedUpdate is a poor place to use GetButtonDown, since it doesn't necessarily run every frame and can miss the event)

        
        if (grounded)
        {
            if (!walk)
                speed = speed * 0.5f * Time.deltaTime;
            else
            {
                if (run)
                    speed = runSpeed;
                else
                    speed = walkSpeed;
            }
        }
        else
        {
            speed = 0;
        }

        if (grounded)
        {
            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (falling)
            {
                falling = false;
                if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                    FallingDamageAlert(fallStartLevel - myTransform.position.y);
            }

            
            moveDirection = this.transform.forward;
            moveDirection.y = 0;
            moveDirection *= speed;
            
        }
        else
        {
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!falling)
            {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }

        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
        
        Plane p = new Plane(controller.transform.right, controller.transform.position);
        float dist = p.GetSide(currentTarget)? 1 : -1 ;
        
        // check if target is behind us. 
        if(controller.transform.TransformPoint(currentTarget).z < 0)
        {
            moveRotation = dist * 90;
        }else
        {
            moveRotation = dist * Vector3.Angle(controller.transform.forward, controller.transform.position - currentTarget);
        }

        if(Mathf.Abs(moveRotation) > 5f)
            controller.transform.Rotate(Vector3.up, moveRotation * Time.deltaTime);



    }
	


    // Store point that we're in contact with for use in FixedUpdate if needed
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contactPoint = hit.point;
    }

    // If falling damage occured, this is the place to do something about it. You can make the player
    // have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
    void FallingDamageAlert(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }

    public void StopWalking()
    {
        speed = 0;
        run = false;
        walk = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(this.transform.position, currentTarget);
    }

}
