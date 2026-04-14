using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] public int enemyHealth = 10;
    [SerializeField] public float enemyMaxSpeed = 10f;
    [SerializeField] public int enemyAttackDamage = 2;

    [Header("RayCast Box")]
    [SerializeField] public GameObject RaycastBox;

    private Animator animator;

    private Ray ray;

    private void Start()
    {
        if(!TryGetComponent<Animator>(out animator))
        {
            Debug.LogError("Skeleton Animator Not Found");
        }
    }

    public void Update()
    {
        ray = new Ray(RaycastBox.transform.position, RaycastBox.transform.forward);
        RaycastHit hit;
        OnDrawGizmos();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.name);

            if(hit.collider.gameObject.CompareTag("Player"))
            {
                Vector3 MovementVector = Vector3.Lerp(transform.position, hit.collider.transform.position, Time.deltaTime);
                animator.SetBool("walk", true);
                transform.Translate(MovementVector);
            }
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }
}
