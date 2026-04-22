using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateGame : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] public GameObject[] Players;
    [SerializeField] public GameObject[] Enemys;
    [SerializeField] public GameObject PlayerSpawn;
    [SerializeField] public GameObject[] EnemySpawns;

    private int DeathCount = 0;

    private GameObject CurrentPlayer;

    private int EnemyTillWin = 20;

    private void Start()
    {
        CurrentPlayer = Instantiate(Players[DeathCount], PlayerSpawn.transform);

        for(int i = 0; i < EnemySpawns.Length; i++)
        {
            Instantiate(Enemys[Random.Range(0, Enemys.Length-1)], EnemySpawns[i].transform);
        }

        GameObject Canvas = GameObject.Find("Canvas");
        PlayerHP EnemyUpdate = Canvas.GetComponent<PlayerHP>();
        EnemyUpdate.UpdateEnemiesLeft(EnemyTillWin);
    }

    public void PlayerDied()
    {
        DeathCount++;
        CurrentPlayer = null;
        if(DeathCount < Players.Length)
            CurrentPlayer = Instantiate(Players[DeathCount], PlayerSpawn.transform);
        else
        {
            GameLose();
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

    public void EnemyDied()
    {
        EnemyTillWin--;
        Instantiate(Enemys[Random.Range(0, Enemys.Length - 1)], EnemySpawns[Random.Range(0, EnemySpawns.Length - 1)].transform);
        GameObject Canvas = GameObject.Find("Canvas");
        PlayerHP EnemyUpdate = Canvas.GetComponent<PlayerHP>();
        EnemyUpdate.UpdateEnemiesLeft(EnemyTillWin);

        if (EnemyTillWin <= 0)
        {
            PlayerWins();
        }
    }

    public void GameLose()
    {
        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayerWins()
    {
        SceneManager.LoadScene(3);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
