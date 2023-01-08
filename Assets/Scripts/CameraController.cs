using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    
    [SerializeField] float zoomSpeed;
    [SerializeField] float smoothSpeed;

    float targetZoomValue = -85f;
    float currentZoomValue = -85f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 scrollDelta = Input.mouseScrollDelta;

        targetZoomValue += scrollDelta.y * zoomSpeed;
        targetZoomValue = Mathf.Clamp(targetZoomValue, minDistance, maxDistance);

        currentZoomValue = Mathf.Lerp(currentZoomValue, targetZoomValue, Time.deltaTime * smoothSpeed);

        transform.localPosition = new Vector3(0f, 0f, currentZoomValue);
        
    }
}
