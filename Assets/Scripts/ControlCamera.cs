using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{
    [Header("OffSet")]
    public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f); // shoulder height

    [Header("Camera")]
    public GameObject Camera;

    [Header("Sensitivity")]
    public float MouseSense = 15f;
    public float JoyStickSense = 180f;

    private void Awake()
    {
        Camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (Camera == null)
            Debug.LogError("Camera Not Found");

        Camera.transform.position = transform.position + pivotOffset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LookMouse(InputAction.CallbackContext context)
    {
        Vector2 lookVector = context.ReadValue<Vector2>();

        float deltaX;

        deltaX = lookVector.x * MouseSense;
        Vector3 look = new Vector3(0f, deltaX, 0f);

        transform.Rotate(look, Space.Self);
    }

    public void Update()
    {
        //Camera.transform.position = transform.position + pivotOffset;
    }

    public void PlayerResetCamera()
    {
        Debug.Log("Camera Pos Reset");
        Camera = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (Camera == null)
            Debug.LogError("Camera Not Found");

        Camera.transform.position = transform.position + pivotOffset;
    }
}
