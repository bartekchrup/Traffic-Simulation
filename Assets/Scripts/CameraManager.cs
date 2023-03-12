using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    private UserInputSystem inputSystem;
    public float movementSpeed = 1;
    public float zoomSpeed = 1;
    public float minZoomLevel = 1;
    public float maxZoomLevel = 10;


    private void Awake() {
        inputSystem = new UserInputSystem();
    }

    void Update()
    {
        // Read movement axis off user input system and adjust camera
        Vector2 cameraMovementVector = inputSystem.User.Move.ReadValue<Vector2>();
        if (cameraMovementVector.x != 0 && cameraMovementVector.y != 0) {

        }
        transformCamera(cameraMovementVector);

        // Read scrool wheel zoom axis
        float zoomFactor = inputSystem.User.Zoom.ReadValue<float>();
        if (zoomFactor != 0) {
            zoomCamera(zoomFactor);
        }
    }

    public void setCameraLocation(Vector3 positionIn)
    {
        transform.position = positionIn;
    }

    private void transformCamera(Vector2 movementVector)
    {
        movementVector *= movementSpeed * Time.deltaTime;
        transform.Translate(movementVector.x, movementVector.y, 0);

    }

    private void zoomCamera(float zoomFactor) {
        // The zoom factor is multiplied by the time since the last frame to ensure smooth zooming
        // 1 is added to normalise it around 1 for use as a multiplier
        // Multiplied by -1 to invert zoom axis
        zoomFactor = 1 + (zoomFactor * -1 * zoomSpeed * Time.deltaTime);
        float cameraSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = Mathf.Clamp(cameraSize *= zoomFactor, minZoomLevel, maxZoomLevel);
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
