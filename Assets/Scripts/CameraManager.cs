using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    private UserInputSystem inputSystem;
    private float movementX;
    private float movementY;
    public float movementSpeed = 1;

    public void setCameraLocation(Vector3 positionIn)
    {
        transform.position = positionIn;
    }

    public void transformCamera(Vector2 movementVector)
    {
        movementVector *= movementSpeed * Time.deltaTime;
        transform.Translate(movementVector.x, movementVector.y, 0);

    }

    private void Awake() {
        inputSystem = new UserInputSystem();
    }

    void FixedUpdate()
    {
        Vector2 cameraMovementVector = inputSystem.User.Move.ReadValue<Vector2>();
        transformCamera(cameraMovementVector);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
