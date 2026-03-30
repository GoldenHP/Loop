using UnityEngine;

public class AnimtorTestor : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    private void Start()
    {
        TryGetComponent<Animator>(out animator);
        TryGetComponent<CharacterController>(out controller);
    }

    private void Update()
    {
        animator.SetBool("run", true);
    }
}
