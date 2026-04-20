using UnityEngine;

public class CreateGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public GameObject[] Players;
    [SerializeField] public GameObject[] Enemys;
    [SerializeField] public GameObject PlayerSpawn;
    [SerializeField] public GameObject[] EnemySpawns;

    private int DeathCount = 0;

    private GameObject CurrentPlayer;

    private void Start()
    {
        Instantiate(Players[DeathCount], PlayerSpawn.transform);

        for(int i = 0; i < EnemySpawns.Length; i++)
        {
            CurrentPlayer = Instantiate(Enemys[Random.Range(0, Enemys.Length-1)], EnemySpawns[i].transform);
        }
    }

    public void PlayerDied()
    {
        DeathCount++;
        CurrentPlayer = null;
        if(DeathCount < Players.Length)
            CurrentPlayer = Instantiate(Players[DeathCount], PlayerSpawn.transform);
        else
        {
            //GameLose();
        }

        ControlCamera cam;
        cam = CurrentPlayer.GetComponent<ControlCamera>();
        if (!cam)
            Debug.Log("Camera Script on Player not found");
        else
        {
            cam.PlayerResetCamera();
        }
    }
}
