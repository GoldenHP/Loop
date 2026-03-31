using UnityEngine;

public class AnimtorTestor : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    private float time = 0;

    private void Start()
    {
        TryGetComponent<Animator>(out animator);
        TryGetComponent<CharacterController>(out controller);
    }

    private void Update()
    {
        /*if(time < 3f)
            animator.SetBool("run", true);

        if (time > 3f)
            animator.SetBool("run", false);*/



        /*animator.SetBool("lightattack", true);

        if(time > .25f) 
            animator.SetBool("lightattack", false);*/

        /*animator.SetBool("jump", true);
        if (time > .25f)
            animator.SetBool("jump", false);*/

        /*animator.SetBool("sprin", true);
        if (time > .25f)
            animator.SetBool("sprin", false);*/

        animator.SetBool("heavyattack", true);
        if (time > 0.25f)
            animator.SetBool("heavyattack", false);

        time += Time.deltaTime;
    }
}
