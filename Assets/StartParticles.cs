using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem smonk;

    private void Update()
    {
        if (!smonk.isPlaying) smonk.Play();
    }
}
