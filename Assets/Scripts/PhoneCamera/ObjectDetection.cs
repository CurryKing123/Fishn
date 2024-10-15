using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.ObjectDetection;
using UnityEngine;

public class ObjectDetection : MonoBehaviour
{
    [SerializeField] float probabilityThreshold = .5f;
    [SerializeField] private ARObjectDetectionManager objectDetectionManager;

    private Color[] colors = new Color[]
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
    }

    private void ObjectDetectionManagerOnMetadataInitialized(ARObjectDetectionModelEventArgs args)
    {
        objectDetectionManager.ObjectDetectionsUpdated += ObjectDetectionManagerOnObjectDetectionUpdated;
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
        var results = args.Results;

        if (results == null)
        {
            return;
        }

        drawRect.ClearRects();

        for (int i = 0; i < results.Count; i++)
        {
            var detection = results[i];
            var categorizations = detection.GetConfidentCategorizations(probabilityThreshold);

            if (categorizations.Count <= 0)
            {
                break;
            }

            categorizations.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));
            var categoryToDisplay = categorizations[0];
            confidence = categoryToDisplay.Confidence;
            rectName = categoryToDisplay.CategoryName;

            int h = Mathf.FloorToInt(canvas.GetComponent<RectTransform>().rect.height);
            int w = Mathf.FloorToInt(canvas.GetComponent<RectTransform>().rect.width);

            var rect = results[i].CalculateRect(w, h, Screen.orientation);

            resultString = $"{rectName}: {confidence}\n";

            Debug.Log(resultString);

            drawRect.CreateRect(rect, colors[i % colors.Length], resultString);
        }
    }
}
