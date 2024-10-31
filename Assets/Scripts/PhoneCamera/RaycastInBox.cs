using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class RaycastInBox : MonoBehaviour
{
    [SerializeField] private ARRaycastManager aRRaycastManager;
    [SerializeField] private ARPointCloudManager aRPointCloudManager;
    [SerializeField] private ObjectDetection objectDetection;
    [SerializeField] private Camera mainCamera;


    private void Update()
    {
        
    }

    public (Vector3 position, Vector3 normal) RaycastFromDetectedPosition(Vector2 rectPosition)
    {
        Ray ray;
        Vector2 convertScreenToRay = new Vector2(rectPosition.x / 2, rectPosition.y / 2);

        ray = mainCamera.ScreenPointToRay(convertScreenToRay);

        RaycastHit[] hits = Physics.RaycastAll(ray, 50f);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                
            }
        }

        return (Vector3.zero, Vector3.zero);
    }
}
