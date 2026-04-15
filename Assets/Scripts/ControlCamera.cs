using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform player;           // drag your Player root here
    public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f); // shoulder height

    [Header("Orbit")]
    public float followSpeed = 10f;  // how fast camera yaw catches player
    public float returnDelay = 1.2f; // seconds idle before auto-returning

    [Header("Sensitivity")]
    public float mouseSensitivity = 0.15f;
    public float joystickSensitivity = 180f;

    [Header("Pitch Limits")]
    public float minPitch = -20f;
    public float maxPitch = 50f;

    [Header("Distance")]
    public float desiredDistance = 4f;
    public float minDistance = 0.5f;

    [Header("Collision")]
    public float collisionRadius = 0.2f;
    public LayerMask collisionMask;      // set to Default (or whatever your geometry is)

    [Header("Invert Y")]
    public bool invertY = false;

    private float _yaw;
    private float _pitch;
    private float _currentDistance;
    private float _idleTimer;
    private float _targetYaw;

    private bool _playerIsMoving;
    private bool _isGamepad;

    private InputAction _lookAction;
    private InputAction _moveAction;

    void Awake()
    {
        var playerInput = player.GetComponent<PlayerInput>();
        _lookAction = playerInput.actions["Look"];
        _moveAction = playerInput.actions["Move"];
    }

    void OnEnable()
    {
        _lookAction.Enable();
        _moveAction.Enable();
        InputSystem.onActionChange += OnActionDeviceChange;
    }

    void OnDisable()
    {
        _lookAction.Disable();
        _moveAction.Disable();
        InputSystem.onActionChange -= OnActionDeviceChange;
    }

    void Start()
    {
        _yaw = player.eulerAngles.y;
        _targetYaw = _yaw;
        _pitch = 15f;
        _currentDistance = desiredDistance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()  // LateUpdate keeps the camera from jittering after physics
    {
        HandleLookInput();
        //HandleAutoFollow();
        ApplyCollision();
        ApplyTransform();
    }

    void HandleLookInput()
    {
        Vector2 look = _lookAction.ReadValue<Vector2>();

        float deltaX;

        if (_isGamepad)
            deltaX = look.x * joystickSensitivity * Time.deltaTime;
        else
            deltaX = look.x * mouseSensitivity;

        // Yaw follows look.x — player turns left/right
        _yaw += deltaX;

        // Pitch is fixed — camera stays level, no up/down look
        _pitch = 15f; // or whatever default angle you want
    }

    void HandleAutoFollow()
    {
        Vector2 move = _moveAction.ReadValue<Vector2>();
        _playerIsMoving = move.magnitude > 0.1f;

        if (_playerIsMoving)
            _idleTimer += Time.deltaTime;
        else
            _idleTimer = 0f;

        // After returnDelay seconds of moving without manual look input,
        // smoothly swing the camera behind the player
        if (_idleTimer >= returnDelay)
        {
            _targetYaw = player.eulerAngles.y;
            _yaw = Mathf.LerpAngle(_yaw, _targetYaw, followSpeed * Time.deltaTime);
        }
    }

    void ApplyCollision()
    {
        Vector3 pivotWorld = player.position + pivotOffset;
        Vector3 direction = transform.position - pivotWorld;

        // SphereCast from pivot toward desired camera position
        if (Physics.SphereCast(pivotWorld, collisionRadius, direction.normalized,
                               out RaycastHit hit, desiredDistance, collisionMask))
        {
            _currentDistance = Mathf.Clamp(hit.distance, minDistance, desiredDistance);
        }
        else
        {
            // Smoothly restore distance when obstruction clears
            _currentDistance = Mathf.Lerp(_currentDistance, desiredDistance, 10f * Time.deltaTime);
        }
    }

    void ApplyTransform()
    {
        Vector3 pivotWorld = player.position + pivotOffset;

        // Use player's yaw + camera's pitch only, no roll
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 offset = rotation * new Vector3(0f, 0f, -_currentDistance);

        transform.position = pivotWorld + offset;
        transform.LookAt(pivotWorld);
    }

    void OnActionDeviceChange(object action, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var inputAction = action as InputAction;
        if (inputAction == null || inputAction.name != "Look") return;

        _isGamepad = inputAction.activeControl?.device is Gamepad;
    }


}
