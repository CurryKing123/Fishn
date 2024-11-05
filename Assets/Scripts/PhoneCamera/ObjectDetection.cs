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
    //[SerializeField] private ARRaycastManager arRaycastManager;
    //[SerializeField] private RaycastInBox raycastInBox;


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

    private void Start()
    {
        objectDetectionManager.enabled = true;
        objectDetectionManager.MetadataInitialized += ObjectDetectionManagerOnMetadataInitialized;

        //raycastInBox = new RaycastInBox();
    }

    private void ObjectDetectionManagerOnMetadataInitialized(ARObjectDetectionModelEventArgs args)
    {
        objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionManagerOnObjectDetectionUpdated;
        pointCloudManager.enabled = true;
    }

    private void OnDestroy()
    {
        objectDetectionManager.MetadataInitialized -= ObjectDetectionManagerOnMetadataInitialized;
        objectDetectionManager.ObjectDetectionsUpdated -= ObjectDetectionManagerOnObjectDetectionUpdated;
        
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
