
using System;
using UnityEngine;
using UnityEngine.UI;

public class HandleCursor : MonoBehaviour
{
    public float dist;
    public Transform mainCam;
    public Sprite defaultSprite;
    public Sprite pickupSprite;
    public Sprite toggleDoorSprite;
    public Sprite toggleBreaker;
    public Sprite pickupItemSprite;
    public Image cursorImage;
    public Sprite crosshair;
    public bool paused;
    public Color color;


    private void Start()
    {
        color = cursorImage.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        { // This function handles the cursor behavior.
            RaycastHit hit; // Cast a ray from the cursor position using the camera's forward direction.
            bool raycastCheck = Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, dist); // Check if the ray hits an object within the maximum distance.
            Debug.DrawLine(mainCam.position, mainCam.position + mainCam.TransformDirection(Vector3.forward) * 200, Color.green);
            if (raycastCheck && !hit.transform.CompareTag("PickUpCube") && !hit.transform.CompareTag("PickUpPolyShape") && !hit.transform.CompareTag("ToggleDoor") && !hit.transform.CompareTag("ToggleBreaker") )
                cursorImage.sprite = defaultSprite; // Change the sprite to the default sprite
            else if ( ( raycastCheck && hit.transform.CompareTag("PickUpCube") ) || ( raycastCheck && hit.transform.CompareTag("PickUpPolyShape")) )  // If the raycast hits an object with the tag "PickUpCube" or "PickUpPolyShape", hide the mainImage and show the secondImage.
                cursorImage.sprite = pickupSprite; // Change the sprite to the pickup sprite
            else if (raycastCheck && hit.transform.CompareTag("PickUpItem")) 
                cursorImage.sprite = pickupItemSprite;
            else if (raycastCheck && hit.transform.CompareTag("ToggleDoor"))  // if the raycast hits a object with the tag "ToggleDoor" change the sprite to toggleDoorSprite;
                cursorImage.sprite = toggleDoorSprite; // Change the Sprite to the toggleDoor sprite.
            else if (raycastCheck && hit.transform.CompareTag("ToggleBreaker")) {
                cursorImage.sprite = toggleBreaker;
            }
        }
        // // Set the alpha value of the material's color based on the value of the 'paused' variable.
        // color.a = paused ? 0f : 1f;
        // cursorImage.material.color = color;
        
    }
    
}
