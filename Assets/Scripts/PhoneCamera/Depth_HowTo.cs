    
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using Niantic.Lightship.AR.Utilities;

    public class Depth_HowTo : MonoBehaviour
    {
        public AROcclusionManager _occlusionManager;

        void Update()
        {
            if (!_occlusionManager.subsystem.running)
            {
                return;
            }
            
            // get the depth texture from AR Foundation
            // it should have the same aspect ratio as the background image
            var depthTexture = _occlusionManager.environmentDepthTexture;
            
            // we can't guarantee the layout of the camera's display matrix because it varies by platform
            // so instead we calculate it ourselves using the CameraMath library
            var displayMatrix = CameraMath.CalculateDisplayMatrix
            (
                depthTexture.width,
                depthTexture.height,
                Screen.width,
                Screen.height,
                XRDisplayContext.GetScreenOrientation()
            );
            
            // Do something with the texture
            // ...

        }
    }