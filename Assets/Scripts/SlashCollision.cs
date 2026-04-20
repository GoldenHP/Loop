using UnityEngine;

public class SlashCollision : MonoBehaviour
{
    [Header("Slash Damage")]
    public int Damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "From Slash");

        EnemyScript Enemy;
        if(!other.TryGetComponent<EnemyScript>(out Enemy))
        {
            Debug.Log("Failed to grab enemy script");
        }
        else
        {
            Enemy.TakeDamage(Damage, transform.parent.gameObject);
            Debug.Log("Enemy Take Damage");
            Enemy = null;
        }
    }
}
