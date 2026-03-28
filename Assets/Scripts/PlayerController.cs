using Unity.Cinemachine;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Specs")]
    public float velocity = 5f;
    public float sprintAddtion = 3.5f;
    public float jumpForce = 18f;
    public float gravity = 9.8f; 
    public float jumpTime = 0.85f;

    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;

    private Vector3 TrueVector;
    private Vector2 MovementVector;
    private CharacterController Controller;
    private Animator animator;

    float jumpElapsedTime = 0;


    private void Start()
    {
        if (!TryGetComponent<CharacterController>(out Controller))
            Debug.LogError("Controller Failed to Load");
      
        if (!TryGetComponent<Animator>(out animator))
            Debug.LogError("Animator Failed to load");
    }

    public void Move(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        isCrouching = !isCrouching;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        isSprinting = !isSprinting;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        isJumping = true;
    }

    private void Update()
    {
        if (animator != null)
        {
            if (isCrouching)
                animator.SetBool("crouch", true);
            else
                animator.SetBool("crouch", false);

            float minimumSpeed = 0.5f;
            if (Controller.velocity.magnitude > minimumSpeed)
                animator.SetBool("run", true);
            else
                animator.SetBool("run", false);

            animator.SetBool("sprint", isSprinting);
        }

        if (Controller.isGrounded)
        {
            animator.SetBool("air", false);
        }
        else
            animator.SetBool("air", true);
    }

    private void FixedUpdate()
    {
        float VeloAdd = 0f;
        if (isSprinting)
            VeloAdd = sprintAddtion;
        if (isCrouching)
            VeloAdd = -(velocity * 0.5f);

        TrueVector.x = MovementVector.x * (velocity + VeloAdd) * Time.deltaTime;
        TrueVector.z = MovementVector.y * (velocity + VeloAdd) * Time.deltaTime;

        if(isJumping)
        {
            TrueVector.y = Mathf.SmoothStep(jumpForce, jumpForce * 0.3f, jumpElapsedTime/jumpTime) * Time.deltaTime;
            jumpElapsedTime += Time.deltaTime;
            if(jumpElapsedTime >= jumpTime)
            {
                isJumping = false;
                jumpElapsedTime = 0f;
            }
        }

        TrueVector.y = TrueVector.y - gravity * Time.deltaTime;

        Controller.Move(TrueVector);
    }
}
