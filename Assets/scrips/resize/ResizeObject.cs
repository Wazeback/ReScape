using System.Collections;
using UnityEngine;

public class ResizeObject : MonoBehaviour
{
    public Transform mainCamera;
    private GameObject pickedObject;
    private Vector3 originalScale;
    private float maxDistance;
    private float originalDistance;
    
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.position, mainCamera.TransformDirection(Vector3.forward), out hit))
            {
                if (pickedObject == null)
                {
                    // Pick up object
                    pickedObject = hit.transform.gameObject;
                    originalScale = pickedObject.transform.localScale;
                    originalDistance = Vector3.Distance(mainCamera.position, pickedObject.transform.position);
                }
                else
                {
                    // Drop object
                    pickedObject = null;
                }
            }
        }

        if (pickedObject != null)
        {
            // Move object away from camera
            RaycastHit wallHit;
            if (Physics.Raycast(mainCamera.position, mainCamera.TransformDirection(Vector3.forward), out wallHit))
            {
                maxDistance = Vector3.Distance(mainCamera.position, wallHit.point) - 0.1f;
                float currentDistance = Vector3.Distance(mainCamera.position, pickedObject.transform.position);
                float scaleFactor = currentDistance / originalDistance;
                pickedObject.transform.localScale = originalScale * scaleFactor;
                pickedObject.transform.position = mainCamera.position + mainCamera.TransformDirection(Vector3.forward) * maxDistance;
            }
        }
    }
}