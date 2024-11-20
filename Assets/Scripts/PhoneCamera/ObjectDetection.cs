using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.ObjectDetection;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField] float probabilityThreshold = .4f;
    [SerializeField] private ARObjectDetectionManager objectDetectionManager;
    [SerializeField] private ARPointCloudManager pointCloudManager;

    private ParticleSystem pointCloudParticle;
    private ParticleSystem.Particle[] particles;
    //[SerializeField] private ARRaycastManager arRaycastManager;
    //[SerializeField] private RaycastInBox raycastInBox;

    private float countdown = 3f;
    private bool stopPoints;

    private List<Vector3> pointLocation;


    private Color[] colors = new[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.white,
        Color.yellow,
        Color.magenta,
        Color.cyan,
        Color.black
    };

    //public static event Action<(string category, Vector2 rectPosition)> fishPosition;

    [SerializeField] private DrawRect drawRect;

    private Canvas canvas;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Update()
    {
        if (stopPoints)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0f)
        {
            pointCloudManager.enabled = false;
        }

        int numParticlesAlive = pointCloudParticle.GetParticles(particles);


        if (particles != null)
        {
            if (pointCloudManager.enabled)
            {
                for (int i = 0; i < numParticlesAlive; i++)
                {
                    Vector3 particlePosition = particles[i].position;
                    pointLocation.Add(particlePosition);
                    //pointLocation.Add(new Vector3(particlePosition.x, particlePosition.y, particlePosition.z));
                    Debug.Log(particlePosition);
                    Debug.Log(numParticlesAlive);
                }
                //Debug.Log(pointLocation);
            }
        }
    }

    private void Start()
    {
        objectDetectionManager.MetadataInitialized += ObjectDetectionManagerOnMetadataInitialized;

        pointCloudParticle = pointCloudManager.pointCloudPrefab.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[pointCloudParticle.main.maxParticles];
        pointLocation = new List<Vector3>();

        //raycastInBox = new RaycastInBox();
    }

    private void ObjectDetectionManagerOnMetadataInitialized(ARObjectDetectionModelEventArgs args)
    {
        objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionManagerOnObjectDetectionUpdated;
        countdown = 3f;
        stopPoints = false;
    }

    private void OnDestroy()
    {
        objectDetectionManager.MetadataInitialized -= ObjectDetectionManagerOnMetadataInitialized;
        objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionManagerOnObjectDetectionUpdated;
        stopPoints = true;
    }

    private void ObjectDetectionManagerOnObjectDetectionUpdated(ARObjectDetectionsUpdatedEventArgs args)
    {
        string resultString = "";
        float confidence = 0;
        string rectName = "";
        var result = args.Results;

        if (result == null)
        {
            return;
        }

        drawRect.ClearRects();

        for (int i = 0; i < result.Count; i++)
        {
            var detection = result[i];
            var categorizations = detection.GetConfidentCategorizations(probabilityThreshold);

            if (categorizations.Count <= 0)
            {
                break;
            }

            categorizations.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
            var categoryToDisplay = categorizations[0];
            confidence = categoryToDisplay.Confidence;
            rectName = categoryToDisplay.CategoryName;

            if (rectName == "fish")
            {
                pointCloudManager.enabled = true;

                int h = Mathf.FloorToInt(canvas.GetComponent<RectTransform>().rect.height);
                int w = Mathf.FloorToInt(canvas.GetComponent<RectTransform>().rect.width);

                var rect = result[i].CalculateRect(w, h, Screen.orientation);


                resultString = $"{rectName}: {confidence}\n";

                drawRect.CreateRect(rect, colors[i % colors.Length], resultString);

                //raycastInBox.RaycastFromDetectedPosition(rect.position);

                //pointCloudManager.enabled = true;

                //fishPosition?.Invoke((categoryToDisplay.CategoryName, rect.position));
            }
        }
    }
}
