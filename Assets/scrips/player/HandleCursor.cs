using System;
using UnityEngine;
using UnityEngine.UI;

public class HandleCursor : MonoBehaviour
{
    public float dist;
    public Transform mainCam;
    public Image mainImage;
    public Image secondImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        { // This function handles the cursor behavior.
            RaycastHit hit; // Cast a ray from the cursor position using the camera's forward direction.
            bool raycastCheck = Physics.Raycast(transform.position, mainCam.TransformDirection(Vector3.forward), out hit, dist);  // Check if the ray hits an object within the maximum distance.
            if (raycastCheck && ( hit.transform.CompareTag("PickUpCube") || hit.transform.CompareTag("PickUpPolyShape")) ) {   // If the raycast hits an object with the tag "PickUpCube" or "PickUpPolyShape", hide the mainImage and show the secondImage.
                mainImage.enabled = false;  
                secondImage.enabled = true;
            }
            else { // If the raycast doesn't hit an object with the tag "pickup", show the mainImage and hide the secondImage.
                mainImage.enabled = true;
                secondImage.enabled = false;
            }
        }
    }
}
