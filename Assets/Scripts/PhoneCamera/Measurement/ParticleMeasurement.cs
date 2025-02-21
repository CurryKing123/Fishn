using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ParticleMeasurement : MonoBehaviour
{
    private ParticleSystem pointCloudParticle;
    private ParticleSystem.Particle[] particles;
    private List<Vector3> pointLocation;
    private bool fishCountdown = false;
    private float countdownMax = 5f;
    private float countdownTimer;

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
                    if (fishCountdown != true)
                    {
                        fishCountdown = true;
                        countdownTimer = countdownMax;
                        StartCountdown();
                    }
                }
                //Debug.Log(pointLocation);
        }
        else
        {
            fishCountdown = false;
        }
    }

    private IEnumerator StartCountdown()
    {
        if (countdownTimer > 0)
        {
            if (fishCountdown)
            {
                countdownTimer --;
                yield return new WaitForSeconds(1.0f);
            }
            else {yield break;}
        }

        else
        {
            foreach (Vector3 pos in pointLocation)
            {
                float minPos;
                float maxPos;
            }
            //Foreach loop on point positions
        }
    }
}
