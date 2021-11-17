using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed;
    private Vector3 pivotRotation;
    public GameObject pivot;
    public GameObject player;

    void FixedUpdate()
    {
        pivot.transform.Rotate(pivotRotation);
        //pivot.transform.position = player.transform.position;
    }

    public void RotateCamera(InputAction.CallbackContext rotation)
    {
        Vector2 tempRotation = rotation.ReadValue<Vector2>();
        pivotRotation = new Vector3 (0, tempRotation.x*rotationSpeed, 0);
    }
}
