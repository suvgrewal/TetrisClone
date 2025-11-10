using UnityEngine;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    public ParticleSystem[] allParticles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allParticles = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play()
    {
        foreach (ParticleSystem ps in allParticles)
        {
            // restart effect from beginning
            ps.Stop();
            ps.Play();
        }
    }
}
