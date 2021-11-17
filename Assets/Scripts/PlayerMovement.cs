using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 movement;
    private Vector3 rotatedMovement;

    private Rigidbody rb;
    public GameObject cameraPivot;
    public float speed;
    public float aSpeed;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rotatedMovement = new Vector3(movement.x, 0, movement.y);
        if (rotatedMovement.x < 0.1f && rotatedMovement.x > -0.1f) rotatedMovement = new Vector3(0, rotatedMovement.y, rotatedMovement.z);
        if (rotatedMovement.z < 0.1f && rotatedMovement.z > -0.1f) rotatedMovement = new Vector3(rotatedMovement.x, rotatedMovement.y, 0);
        rotatedMovement = cameraPivot.transform.rotation * rotatedMovement;
        rb.AddForce(rotatedMovement*aSpeed, ForceMode.Acceleration);

        //Clamp Speed
        //X axis
        if (rotatedMovement.x > 0)
        {
            if (rb.velocity.x > rotatedMovement.x*speed)
            {
                rb.velocity = new Vector3(rotatedMovement.x*speed, rb.velocity.y, rb.velocity.z);
            }
        }
        else if (rotatedMovement.x < 0)
        {
            if (rb.velocity.x < rotatedMovement.x*speed)
            {
                rb.velocity = new Vector3(rotatedMovement.x*speed, rb.velocity.y, rb.velocity.z);
            }
        }

        //Y Axis
        if (rotatedMovement.z > 0)
        {
            if (rb.velocity.z > rotatedMovement.z*speed)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rotatedMovement.z*speed);
            }
        }
        else if (rotatedMovement.z < 0)
        {
            if (rb.velocity.z < rotatedMovement.z*speed)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rotatedMovement.z*speed);
            }
        }

    }

    public void Movement(InputAction.CallbackContext stickMovement)
    {
        movement = stickMovement.ReadValue<Vector2>();
    }
}
