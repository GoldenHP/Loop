using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHP : MonoBehaviour
{
    [Header("PLayer HP Text")]
    public TMP_Text PlayerHealth;
    public TMP_Text EnemiesLeft;

    [Header("Game Objects")]
    public GameObject PlayerUI;
    public GameObject MenuUI;

    public void UpdateHealth(int HealthString)
    {
        PlayerHealth.text = "HP: " + HealthString.ToString();
    }

    public void UpdateEnemiesLeft(int EnemiesString) 
    {
        EnemiesLeft.text = "Enemies Left: " + EnemiesString.ToString();
    }

    public void OpenCloseMenu(InputAction.CallbackContext context)
    {
        PlayerUI.SetActive(!PlayerUI.activeSelf);
        MenuUI.SetActive(!MenuUI.activeSelf);

        if (PlayerUI.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (MenuUI.activeSelf) 
        {
            Cursor.lockState= CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
