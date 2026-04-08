using Unity.Cinemachine;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Specs")]
    public float velocity = 5f;
    public float sprintAddtion = 3.5f;
    public float jumpForce = 18f;
    public float gravity = 9.8f; 
    public float jumpTime = 0.25f;
    public float attackTime = 1f;
    public float runTime = 1f;
    public float LightAttackVFXDelayTime = 0.5f;
    public float HeavyAttackVFXDelayTime = 0.5f;

    private bool isJumping = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool isLightAttacking = false;
    private bool isHeavyAttacking = false;
    private bool isRunning = false;

    private Vector3 TrueVector;
    private Vector2 MovementVector;
    private CharacterController Controller;
    private Animator animator;
    private VFXScript VFX;

    private float jumpElapsedTime = 0f;
    private float runElapsedTime = 0f;

    private bool wasJumping = false;
    private bool wasRunning = false;

    private bool LightAttackCountDown = false;
    private bool HeavyAttackCountDown = false;
    private float LightAttackCountDownTimer = 0f;
    private float HeavyAttackCountDownTimer = 0f;

    
    private void Start()
    {
        if (!TryGetComponent<CharacterController>(out Controller))
            Debug.LogError("Controller Failed to Load");
      
        if (!TryGetComponent<Animator>(out animator))
            Debug.LogError("Animator Failed to load");

        if (!TryGetComponent<VFXScript>(out VFX))
            Debug.LogError("VFX Script Failed to load");
    }

    public void Move(InputAction.CallbackContext context)
    {
        MovementVector = context.ReadValue<Vector2>();
        isRunning = true;
        animator.SetBool("run", isRunning);

        bool foward = false;
        bool backwards = false;
        bool left = false;
        bool right = false;

        if (MovementVector.x > 0f)
            right = true;
        if (MovementVector.x < 0f)
            left = true;
        if (MovementVector.y > 0f)
            foward = true;
        if (MovementVector.y < 0f)
            backwards = true;

        animator.SetBool("Forward", foward);
        animator.SetBool("Backwards", backwards);
        animator.SetBool("Left", left);
        animator.SetBool("Right", right);
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
        wasJumping = true;
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        isLightAttacking = true;
    }
    
    public void HeavyAttack(InputAction.CallbackContext context)
    {
        isHeavyAttacking = true;
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("crouch", isCrouching);

            animator.SetBool("sprin", isSprinting);

            animator.SetBool("lightattack", isLightAttacking);

            animator.SetBool("heavyattack", isHeavyAttacking);

            animator.SetBool("jump", isJumping);
        }
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

        if(isJumping || wasJumping)
        {
            TrueVector.y = Mathf.SmoothStep(jumpForce, jumpForce * 0.3f, jumpElapsedTime/jumpTime) * Time.deltaTime;
            jumpElapsedTime += Time.deltaTime;
            isJumping = false;
            if(jumpElapsedTime > jumpTime)
            {
                wasJumping = false;
                jumpElapsedTime = 0f;
            }
        }

        if (isLightAttacking)
        {
            isLightAttacking = false;
            LightAttackCountDown = true;
        }

        if(LightAttackCountDown)
            LightAttackCountDownTimer += Time.deltaTime;


        if (LightAttackCountDownTimer >= LightAttackVFXDelayTime)
        {
            VFX.SmallAttack();
            LightAttackCountDownTimer = 0f;
            LightAttackCountDown = false;
        }

        if (isHeavyAttacking)
        {
            isHeavyAttacking = false;
            HeavyAttackCountDown = true;
        }

        if(HeavyAttackCountDown)
            HeavyAttackCountDownTimer += Time.deltaTime;

        if (HeavyAttackCountDownTimer >= HeavyAttackVFXDelayTime)
        {
            VFX.BigAttack();
            HeavyAttackCountDownTimer = 0f;
            HeavyAttackCountDown = false;
        }

        TrueVector.y = TrueVector.y - gravity * Time.deltaTime;

        Controller.Move(TrueVector);

        if(MovementVector == Vector2.zero)
        {
            isRunning = false;
            animator.SetBool("run", isRunning);
        }

        if(wasRunning && isRunning == false)
        {
            runElapsedTime += Time.deltaTime;
            if(runElapsedTime > runTime)
            {
                runElapsedTime = 0f;
                isRunning = true;
            }
        }
    }
}
