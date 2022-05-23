using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserTrail_TrailHead : MonoBehaviour
{
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    public bool requiresCollection;

    //On Enable Setup
    private void OnEnable()
    {
        particleSystems.Clear();
        FindAllParticleSystems();
    }

    //Find All Particle Systems
    private void FindAllParticleSystems()
    {
        foreach (ParticleSystem ps in GetComponentsInChildren<ParticleSystem>())
        {
            particleSystems.Add(ps);
        }
    }

    //Move to requested coordinates
    public void MoveToLocation(Vector3 location)
    {
        transform.position = location;
    }

    //Play all animations
    public void PlayAllAnimations()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

    //Stop playing all animations - but do not clear
    public void StopPlayingAnimations()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    //Stop playing all animations and clear
    public void StopPlayingAnimationsAndClear()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
