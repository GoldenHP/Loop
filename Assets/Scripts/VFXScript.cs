using UnityEngine;
using System.Collections;

public class VFXScript : MonoBehaviour
{
    [Header("Yes")]
    [SerializeField] public ParticleSystem[] particleSystems;
    [SerializeField] public GameObject SmallSlash;
    [SerializeField] public GameObject BigSlash;

    [Header("Slash Timer")]
    [SerializeField] public float BigSlashAttackTime = 1f;
    [SerializeField] public float SmallSlashAttackTime = 1f;

    private Coroutine coroutine;

    private GameObject BigSlashInstance;
    private GameObject SmallSlashInstance;

    private float BigSlashTimer = 0f;
    private float SmallSlashTimer = 0f;

    private bool BigSlashCountdown = false;
    private bool SmallSlashCountdown = false;

    public bool canHeavyAttack;
    public bool canLightAttack;

    private void Start()
    {
        canHeavyAttack = true;
        canLightAttack = true;
    }

    public void BigAttack()
    {
        Transform SlashTransformSpawn = transform;
        SlashTransformSpawn.Translate(Vector3.forward * 5, Space.Self);

        if (!BigSlashCountdown)
        {
            BigSlashInstance = GameObject.Instantiate(BigSlash, SlashTransformSpawn);
            BigSlashCountdown = true;
        }
    }

    public void SmallAttack() 
    {
        Transform SlashTransformSpawn = transform;
        SlashTransformSpawn.Translate(Vector3.forward * 2, Space.Self);

        if (!SmallSlashCountdown)
        {
            SmallSlashInstance = GameObject.Instantiate(SmallSlash, SlashTransformSpawn);
            SmallSlashCountdown = true;
        }
    }

    public void FixedUpdate()
    {
        if (BigSlashCountdown)
            BigSlashTimer += Time.deltaTime;

        if(SmallSlashCountdown)
            SmallSlashTimer += Time.deltaTime;

        if(BigSlashTimer >= BigSlashAttackTime)
        {
            BigSlashCountdown = false;
            Destroy(BigSlashInstance);
            BigSlashTimer = 0f;
            canHeavyAttack = true;
        }

        if(SmallSlashTimer >= SmallSlashAttackTime)
        {
            SmallSlashCountdown = false;
            Destroy(SmallSlashInstance);
            SmallSlashTimer = 0f;
            canLightAttack = true;
        }
    }
}
