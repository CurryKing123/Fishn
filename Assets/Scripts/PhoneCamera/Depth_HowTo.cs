    
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using Niantic.Lightship.AR.Utilities;
    using UnityEngine.UI;
using Niantic.Lightship.AR.Occlusion;

public class Depth_HowTo : MonoBehaviour
    {
        public AROcclusionManager _occlusionManager;
        public LightshipOcclusionExtension _occlusionExtension;
        public RawImage _rawImage;
        public Material _material;

          
    void Update()
    {
        if (!_occlusionManager.subsystem.running)
        {
            return;
        }

        //add our material to the raw image
        _rawImage.material = _material;
        
        // set the depth texture and display matrix
        var depthTexture = _occlusionExtension.DepthTexture;
        var displayMatrix = _occlusionExtension.DepthTransform;

        //set our variables for the shader
        //NOTE: Updating the depth texture needs to happen in the Update() function
        _rawImage.material.SetTexture("_DepthTex", depthTexture);
        _rawImage.material.SetMatrix("_DepthTransform", displayMatrix);
    }

}