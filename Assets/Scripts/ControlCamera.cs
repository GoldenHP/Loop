using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform Player;
    public Vector3 pivotOffset = new Vector3 (0f, 1.6f, 0f);

    [Header("Orbit")]
    public float followSpeed = 10f;
    public float returnDelay = 1.2f;

    [Header("Sensitivity")]
    public float mouseSense = 0.15f;
    public float joyStickSense = 180f;

    [Header("Pitch Limits")]
    public float minPinch = -80f;
    public float maxPinch = 80f;

    [Header("Distance")]
    public float DesiredDistance = 4f;
    public float minDistance = 0.5f;

    [Header("Invert Y")]
    public bool invertY = false;

    [Header("Camera")]
    public GameObject Camera;

    [Header("Collision")]
    public float collisionRadius = 0.2f;
    public LayerMask collisonMask;

    private float yaw;
    private float pitch;
    private float currentDistance;
    private float targetYaw;


    private Vector2 cameraMovement;

    private void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        if(Camera == null)
        {
            Debug.LogError("Couldnt Find Main Cmaera");
        }

        yaw = Player.eulerAngles.y;
        targetYaw = yaw;
        pitch = 15f;
        currentDistance = DesiredDistance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void CameraMouseMove(InputAction.CallbackContext context)
    {
        cameraMovement = context.ReadValue<Vector2>();

        float deltaX, deltaY;

        deltaX = cameraMovement.x * mouseSense;
        deltaY = cameraMovement.y * mouseSense;

        if(invertY)
            deltaY = -deltaY;

        yaw += deltaX;
        pitch += deltaY;
        pitch = Mathf.Clamp(pitch, minPinch, maxPinch);

        Camera.transform.rotation = Quaternion.Euler(yaw, pitch, 0f);
    }

    public void CameraControllerMove(InputAction.CallbackContext context) 
    {
        cameraMovement = context.ReadValue<Vector2>();

        float deltaX, deltaY;
        deltaX = cameraMovement.x * joyStickSense;
        deltaY = cameraMovement.y * joyStickSense;

        if (invertY)
            deltaY = -deltaY;

        yaw += deltaX;
        pitch += deltaY;
        pitch = Mathf.Clamp(pitch, minPinch, maxPinch);

        Camera.transform.rotation = Quaternion.Euler(yaw, pitch, 0f);
    }
}
