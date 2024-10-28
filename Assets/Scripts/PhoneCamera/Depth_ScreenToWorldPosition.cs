using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.ObjectDetection;
using Niantic.Lightship.AR.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Depth_ScreenToWorldPosition : MonoBehaviour
{
    [SerializeField]
    private AROcclusionManager _occlusionManager;

    private XRCpuImage? _depthImage;
    private Matrix4x4 _displayMatrix;
    private ScreenOrientation? _latestScreenOrientation;

    private void Update()
    {
        UpdateImage();
        UpdateDisplayMatrix();
    }

    private void UpdateImage()
    {
        if(!_occlusionManager.subsystem.running)
        {
            return;
        }

        if (_occlusionManager.TryAcquireEnvironmentDepthConfidenceCpuImage(out var image))
        {
            // Dispose old image
            _depthImage?.Dispose();

            // Cache new image
            _depthImage = image;
        }
    }

    private void UpdateDisplayMatrix()
    {
    // Make sure we have a valid depth image
    if (_depthImage is {valid: true})
    {
        // The display matrix only needs to be recalculated if the screen orientation changes
        if (!_latestScreenOrientation.HasValue ||
            _latestScreenOrientation.Value != XRDisplayContext.GetScreenOrientation())
        {
            _latestScreenOrientation = XRDisplayContext.GetScreenOrientation();
            _displayMatrix = CameraMath.CalculateDisplayMatrix(
                _depthImage.Value.width,
                _depthImage.Value.height,
                Screen.width,
                Screen.height,
                _latestScreenOrientation.Value,
                invertVertically: true);
        }
    }
    }
    public void MeasureFish(ARObjectDetectionsUpdatedEventArgs args)
    {
        
    }
}
