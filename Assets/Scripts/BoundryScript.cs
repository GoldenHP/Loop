using UnityEngine;

public class BoundryScript : MonoBehaviour
{
    CreateGame Game;

    private void Start()
    {
        GameObject setter = GameObject.Find("GameSetter");
        if (!setter)
            Debug.Log("Setter Not Found");
        else
        {
            Game = setter.GetComponent<CreateGame>();
            if (!Game)
                Debug.Log("Game Creator Script not grabbed");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PLayer Hit Lower Boundry");
            PlayerController player = other.GetComponent<PlayerController>();
            player.Death();
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            enemy.EnemyDeath();
        }
        
    }
}
