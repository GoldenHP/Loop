using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] public int enemyMaxHealth = 20;
    [SerializeField] public float enemyMaxSpeed = 10f;
    [SerializeField] public int enemyAttackDamage = 2;
    [SerializeField] public float enemyAttackLength = 2f;

    [Header("Debug Colors")]
    public Color HitColor = Color.red;
    public Color MissColor = Color.green;

    [Header("Looking Around Settings")]
    public float LookSpeed = 30f;
    public float MinAngle = -60f;
    public float MaxAngle = 60f;
    public float TimeTillLookHarder = 20f;

    [Header("Detection")]
    public LayerMask PlayerLayer;

    [Header("Movement Ranges")]
    public float AttackRange = 2f;
    public float WalkRange = 10f;

    private Animator animator;
    private GameObject TrackingTarget;

    private float currentAngle = 0f;
    public float targetAngle = 0f;
    private Quaternion startRotation;

    private float timeLookingForPlayer=0f;
    private bool lookingHarder = false;

    private int enemyCurrentHealth;

    private bool isAttacking = false;
    private float enemyAttacktime = 0f;

    private void Start()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Skeleton Animator Not Found");
        }

        startRotation = transform.rotation;
        targetAngle = MaxAngle;

        enemyCurrentHealth = enemyMaxHealth;
    }

    public void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f)) 
        {
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * hit.distance, HitColor);
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.5f, HitColor);

            if(hit.collider.gameObject.CompareTag("Player"))
            {
                TrackingTarget = hit.collider.gameObject;
                if(lookingHarder)
                    StopLookingHarder();
            }
        }
        else
        {
            TrackingTarget = null;
        }

        EnemyMove(hit);

        if(isAttacking)
            enemyAttacktime += Time.deltaTime;
        if(enemyAttacktime > enemyAttackLength)
        {
            enemyAttacktime = 0;
            isAttacking = false;
            animator.SetBool("stab", false);
            TrackingTarget = null;
        }
    }

    public void EnemyMove(RaycastHit hit)
    {
        if(TrackingTarget != null)
        {
            if (hit.distance < AttackRange)
            {
                Attack();
            }
            else if (hit.distance < WalkRange)
            {
                animator.SetBool("walk", true);
                animator.SetBool("run", false);

                Vector3 EnemyMovementVector = Vector3.Lerp(transform.position, TrackingTarget.transform.position, Time.deltaTime / 2f);
                transform.position = EnemyMovementVector;
            }
            else
            {
                animator.SetBool("run", true);
                animator.SetBool("walk", false);

                Vector3 EnemyMovementVector = Vector3.Lerp(transform.position, TrackingTarget.transform.position, Time.deltaTime);
                transform.position = EnemyMovementVector;
            }
        }
        if(TrackingTarget == null)
        {
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            LookAround();
        }
    }

    public void LookAround()
    {
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, LookSpeed * Time.deltaTime);
        transform.rotation = startRotation * Quaternion.Euler(0f, currentAngle, 0f);

        if(Mathf.Approximately(currentAngle, targetAngle))
            targetAngle = (targetAngle == MaxAngle) ?  MinAngle : MaxAngle;

        timeLookingForPlayer += Time.deltaTime;
        if(timeLookingForPlayer > TimeTillLookHarder)
        {
            MinAngle -= 30f;
            MaxAngle += 30f;
            lookingHarder = true;
        }
    }

    public void StopLookingHarder()
    {
        timeLookingForPlayer = 0f;
        MinAngle += 30f;
        MaxAngle -= 30f;
        lookingHarder = false;
    }

    public void TakeDamage(int DamageDealt, GameObject Player)
    {
        enemyCurrentHealth -= DamageDealt;

        TrackingTarget = Player;

        if(enemyCurrentHealth <= 0)
            EnemyDeath();
    }

    public void EnemyDeath()
    {
        CreateGame gameCreator;
        GameObject creator = GameObject.Find("GameSetter");

        gameCreator = creator.GetComponent<CreateGame>();
        if (!creator)
            Debug.Log("Couldnt Find Game Setter Object - Enemy");
        if (!gameCreator)
            Debug.Log("Couldnt Find Game Creator - Enemy");
        else
        {
            gameCreator.EnemyDied();
        }

        Destroy(gameObject);
    }

    public void Attack()
    {
        animator.SetBool("run", false);
        animator.SetBool("walk", false);
        animator.SetBool("stab", true);
        isAttacking = true;
    }
}
