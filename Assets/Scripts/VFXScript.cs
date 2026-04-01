using UnityEngine;
using System.Collections;

public class VFXScript : MonoBehaviour
{
    [Header("Yes")]
    [SerializeField] public ParticleSystem[] particleSystems;
    [SerializeField] public GameObject SmallSlash;
    [SerializeField] public GameObject BigSlash;

    private Coroutine coroutine;

    IEnumerator PlayParticles()
    {
        while (coroutine != null) 
        {
            foreach(ParticleSystem particle in particleSystems)
            {
                particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            yield return new WaitForSeconds(1f);

            foreach(ParticleSystem particle in particleSystems)
            {
                particle.Play(true);
            }

            yield return new WaitForSeconds(1.5f);
        }
    }
}
