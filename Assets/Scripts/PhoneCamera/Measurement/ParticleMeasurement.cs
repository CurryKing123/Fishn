using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ParticleMeasurement : MonoBehaviour
{
    private ParticleSystem pointCloudParticle;
    private ParticleSystem.Particle[] particles;
    private List<Vector3> pointLocation;

    void Start()
    {
        pointCloudParticle = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[pointCloudParticle.main.maxParticles];
        pointLocation = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        int numParticlesAlive = pointCloudParticle.GetParticles(particles);
        
        if (particles != null)
        {
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    Vector3 particlePosition = particles[i].position;
                    pointLocation.Add(particlePosition);
                    //pointLocation.Add(new Vector3(particlePosition.x, particlePosition.y, particlePosition.z));
                    Debug.Log(particlePosition);
                }
                //Debug.Log(pointLocation);
                Debug.Log(numParticlesAlive);
        }
    }
}
