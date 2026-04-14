using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] public int enemyHealth = 10;
    [SerializeField] public float enemyMaxSpeed = 10f;
    [SerializeField] public int enemyAttackDamage = 2;

    [Header("Debug Colors")]
    public Color HitColor = Color.red;
    public Color MissColor = Color.green;

    [Header("Looking Around Settings")]
    public float LookSpeed = 30f;
    public float MinAngle = -60f;
    public float MaxAngle = 60f;

    [Header("Detection")]
    public LayerMask PlayerLayer;

    private Animator animator;
    private GameObject TrackingTarget;

    private float currentAngle = 0f;
    public float targetAngle = 0f;
    private Quaternion startRotation;

    private void Start()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Skeleton Animator Not Found");
        }

        startRotation = transform.rotation;
        targetAngle = MaxAngle;
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

                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    TrackingTarget = hit.collider.gameObject;
                }
                else
                {
                    TrackingTarget = null;
                }
            }
        }

        EnemyMove(hit);
    }

    public void EnemyMove(RaycastHit hit)
    {
        if(TrackingTarget != null)
        {
            if (hit.distance < 5f)
            {
                //Attack()
            }
            else if (hit.distance < 30f)
            {
                animator.SetBool("walk", true);
                animator.SetBool("run", false);

                Vector3 EnemyMovementVector = Vector3.Lerp(transform.position, TrackingTarget.transform.position, Time.deltaTime / 10f);
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
            LookAround();
        }
    }

    public void LookAround()
    {
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, LookSpeed * Time.deltaTime);
        transform.rotation = startRotation * Quaternion.Euler(0f, currentAngle, 0f);

        if(Mathf.Approximately(currentAngle, targetAngle))
            targetAngle = (targetAngle == MaxAngle) ?  MinAngle : MaxAngle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerLightAttack") 
        {
        
        }

        if(collision.gameObject.tag == "PLayerHeavyAttack")
        {

        }
    }
}
