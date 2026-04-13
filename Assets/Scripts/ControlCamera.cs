using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSense = 0.15f;
    public float joyStickSense = 180f;

    [Header("Pitch Limits")]
    public float minPinch = -80f;
    public float maxPinch = 80f;

    [Header("Invert Y")]
    public bool invertY = false;

    [Header("Camera")]


    private float _yaw;
    private float _pitch;

    private InputAction _lookAction;
    private bool isGamepad;

    private Vector2 cameraMovement;

    public void CameraMouseMove(InputAction.CallbackContext context)
    {
        cameraMovement = context.ReadValue<Vector2>();

        float deltaX, deltaY;

        deltaX = cameraMovement.x * mouseSense;
        deltaY = cameraMovement.y * mouseSense;

        if(invertY)
            deltaY = -deltaY;

        _yaw += deltaX;
        _pitch += deltaY;
        _pitch = Mathf.Clamp(_pitch, minPinch, maxPinch);

        transform.rotation = Quaternion.Euler(_yaw, _pitch, 0f);
    }
}
