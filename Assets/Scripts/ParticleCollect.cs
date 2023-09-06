using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollect : MonoBehaviour
{
    
    [SerializeField] private Transform TargetPointSF;
    private ParticleSystem _particle;
    private void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_particle.particleCount];
        int count = _particle.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            float lifePercent = 1 - (particles[i].remainingLifetime / particles[i].startLifetime);

            if (lifePercent > 0.8f)
            {
                particles[i].position = Vector3.Lerp(particles[i].position, TargetPointSF.position, (lifePercent - 0.8f) * 2);
            }
        }

        _particle.SetParticles(particles, count);
    }
}
