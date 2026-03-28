using Unity.Cinemachine;
using UnityEngine;

public class CinemachineFindPlayer : MonoBehaviour
{
    public CinemachineCamera VirtualCamera;
    private GameObject Player;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");

        if(Player)
        {
            VirtualCamera.Follow = Player.transform;
            VirtualCamera.LookAt = Player.transform;
        }
        
    }
}
