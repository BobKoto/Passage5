using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticlesOnVisibility : MonoBehaviour
{//Component of VisibilityMarkerCollider //play particles only when Player is inside a triggered box collider. 
    public ParticleSystem myParticleSystem; // Reference to the Particle System
    bool particleSystemIsPlaying;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Object went inside the collider, play the particle system
            if (!particleSystemIsPlaying)
            {
                PlayParticleSystem();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Object left the collider, stop the particle system
            StopParticleSystem();
        }

    }
    void PlayParticleSystem()
    {
        if (myParticleSystem != null)
        {
            particleSystemIsPlaying = true;
            myParticleSystem.Play();
        }
    }
    void StopParticleSystem()
    {
        if (myParticleSystem != null)
        {
            particleSystemIsPlaying = false;
            myParticleSystem.Stop();
        }
    }
}