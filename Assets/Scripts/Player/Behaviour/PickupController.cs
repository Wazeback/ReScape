using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// TODO: Set a min distance away from the camera so that the object wont fill the screen:: DONE
// TODO: Reminder to learn the way the the mesh.verticies is returned for even better filter.

public class PickupController  : MonoBehaviour
{
    public Transform mainCam;

    private GameObject obj;
    private Rigidbody objRb;
    private GameObject pickObj;

    private float[] distances;
    private Vector3[] corners;
    
    private bool resize;
    private bool polyShape;
    
    public float dist;

    private Vector3 direction;

    // Handles Cursor and raycasting for resizing if a object has been picked up
    private void FixedUpdate()
    {

        if (resize) { // If an object is currently being resized, handle the resizing
            if (polyShape) {  // Get the corners of the object being resized.
                corners = GetNonCubicObjectPoints(); }
            else {
                corners = GetCubicObjectPoints(); }
            
            float initialDistance = Vector3.Distance(mainCam.position, obj.transform.position); // Calculate the initial distance between the camera and the object.
            distances = new float[corners.Length];
            RaycastHit hit;
            int i = 0;
           
            foreach (Vector3 corner in corners) { // For each corner of the object, calculate its distance from the camera, taking into account collisions with other objects.
                var distA = (corner - mainCam.position);
                var dirA = distA / distA.magnitude;
                Vector3 direction = dirA;
                
                
                // Debug.Log(dirA);
                if (mainCam.position.z > corner.z)
                {
                    Debug.Log(mainCam.position);
                    Debug.Log("-----------------");
                    Debug.Log(corner);
                    Debug.Log("-----------------");
                }

                if (Physics.Raycast(mainCam.position, direction, out hit, Mathf.Infinity, ~(1 << obj.layer))) {
                    distances[i] = Vector3.Distance(mainCam.position, hit.point);
                    i++; 
                }
                Debug.DrawLine(mainCam.position, corner + direction * dist, Color.red, 0.1f); // Draw a red line to visualize the raycast used to determine the distance to the object.
            }
            
            float shortestValue = Mathf.Min(distances);  // Find the shortest distance calculated in the previous step.
            
                obj.transform.position = // Move the object so that it is at the shortest distance from the camera, but in the same direction as its original position.
                    mainCam.position + mainCam.forward * 
                    (shortestValue - Vector3.Distance(obj.transform.position, 
                        corners[Array.IndexOf(distances, shortestValue)]));



                float scaleFactor = Vector3.Distance(mainCam.position, obj.transform.position) / initialDistance; // Calculate the scale factor based on the new distance between the camera and the object, and apply it to the object's scale and mass.
            obj.transform.localScale *= scaleFactor;
            obj.GetComponent<Rigidbody>().mass *= scaleFactor;
        }
    }
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E)) {  // Check if the 'E' key is pressed
            if (obj) {  // If the player is already holding an object, drop it
                Transform objTrans = objRb.transform;
                resize = false; // Stop resizing the object
                objTrans.gameObject.layer =
                    LayerMask.NameToLayer("Default"); // Set the object's layer back to the default layer
                objRb.useGravity = true; // Enable gravity for the object
                objRb.constraints = RigidbodyConstraints.None; // Remove any constraints on the object's movement
                objRb.detectCollisions = true;
                objTrans.parent = null; // Remove the object's parent
                obj = null; // Clear the reference to the object
            }

            else {
                // If not, check if the player is looking at a pickup object within reach and pick it up
                RaycastHit hit; // Cast a ray forward from the player's position to see if there's a pickup object within reach
                bool raycast = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward),
                    out hit, dist);
                if (raycast && (hit.transform.gameObject.CompareTag("PickUpCube") || hit.transform.gameObject.CompareTag("PickUpPolyShape"))) {
                    pickObj = hit.transform.gameObject;
                    pickObj.layer = LayerMask.NameToLayer("WhatIsPickedUp");
                    objRb = pickObj.GetComponent<Rigidbody>(); // Get the object's Rigidbody component and disable gravity and freeze all constraints
                    objRb.useGravity = false;
                    objRb.constraints = RigidbodyConstraints.FreezeAll;
                    objRb.detectCollisions = false;
                    objRb.transform.parent = mainCam; // Set the object's parent to the main camera so it moves with the camera 
                    obj = pickObj; // Set the object as the currently picked object and enable resizing
                    resize = true;
                    if (pickObj.CompareTag("PickUpPolyShape")) // Set a Bool so the FixedUpdate knows what type of shape it is dealing with
                        polyShape = true;
                    else
                        polyShape = false;
                }
            }
        }
    }


    // Returns an array of 30 Vector3 points that represent the corners, edges, and midpoints of a given object.
    // The array is used for collision detection and object manipulation.
    private Vector3[] GetCubicObjectPoints()
    {
        Renderer compRender = obj.GetComponent<Renderer>();
        corners = new Vector3[30];  // Initialize an array of size 30 to hold all points.
        Transform objPos = obj.transform;  // Get the object's position and rotation.
        Vector3 center = compRender.bounds.center;  // Calculate the center point of the object.
        Quaternion referenceRotation = objPos.rotation.normalized;  // Calculate the reference rotation of the object.
        obj.transform.rotation = Quaternion.Euler(0,0,0);
        Vector3 extents = compRender.bounds.extents;  // Calculate the extents of the object.
        obj.transform.rotation = referenceRotation;
        
        for (UInt16 it = 0; it <= 7; it++) // corners of the object, with an index scheme that allows for bitwise operations
            corners[it] = center + referenceRotation * new Vector3(
                extents.x*(1 - (4 & it)/2), // x-coordinate of the corner
                extents.y*(1 - (2 & it)),   // y-coordinate of the corner
                extents.z*(1 - (1 & it)*2)  // z-coordinate of the corner
                );
            
        // center planes of the object
        corners[8] = center + referenceRotation * new Vector3(extents.x, 0, 0);
        corners[9] = center + referenceRotation * new Vector3(-extents.x, 0, 0);
        corners[10] = center + referenceRotation * new Vector3(0, extents.y, 0);
        corners[11] = center + referenceRotation * new Vector3(0, -extents.y, 0);
        corners[12] = center + referenceRotation * new Vector3(0, 0, extents.z);
        corners[13] = center + referenceRotation * new Vector3(0, 0, -extents.z);
        
        // middle of every corner of the object
        corners[14] = center + referenceRotation * new Vector3(extents.x, 0, extents.z);
        corners[15] = center + referenceRotation * new Vector3(-extents.x, 0, extents.z);
        corners[16] = center + referenceRotation * new Vector3(0, extents.y, extents.z);
        corners[17] = center + referenceRotation * new Vector3(0, -extents.y, extents.z);
        corners[18] = center + referenceRotation * new Vector3(extents.x, 0, -extents.z);
        corners[19] = center + referenceRotation * new Vector3(-extents.x, 0, -extents.z);
        corners[20] = center + referenceRotation * new Vector3(0, extents.y, -extents.z);
        corners[21] = center + referenceRotation * new Vector3(0, -extents.y, -extents.z);
            
        // corners of the top and bottom faces of the object
        corners[22] = center + referenceRotation * new Vector3(extents.x, extents.y, 0);
        corners[23] = center + referenceRotation * new Vector3(-extents.x, extents.y, 0);
        corners[24] = center + referenceRotation * new Vector3(0, extents.y, extents.z);
        corners[25] = center + referenceRotation * new Vector3(0, extents.y, -extents.z);
        corners[26] = center + referenceRotation * new Vector3(extents.x, -extents.y, 0);
        corners[27] = center + referenceRotation * new Vector3(-extents.x, -extents.y, 0);
        corners[28] = center + referenceRotation * new Vector3(0, -extents.y, extents.z);
        corners[29] = center + referenceRotation * new Vector3(0, -extents.y, -extents.z);
        
        return corners;
    }

    // Returns an array of x amount of Vector3 points that represent the amount of "corners" in a mesh.
    // The array is used for collision detection and object manipulation.
    // TODO: This method is very expensive to use and should only be used in some cases. It cast around 3 time the amount of rays it should cast and generates a fps drop of around 20fps per picked up object.
    private Vector3[] GetNonCubicObjectPoints()
    {
        corners = obj.GetComponent<MeshFilter>().mesh.vertices; // Get object's vertices in local space
        HashSet<Vector3> uniquePoints = new HashSet<Vector3>();
        // Transform vertices to world space with current rotation and add them to the uniquePoints hashset
        for (int i = 0; i < corners.Length; i++) {
            Vector3 worldPoint = obj.transform.TransformPoint(corners[i]);
            if (!uniquePoints.Contains(worldPoint)) 
                uniquePoints.Add(worldPoint);
        }

        // Array a = new List<Vector3>(uniquePoints).ToArray();
        //
        // Debug.Log( a.Length);
        return new List<Vector3>(uniquePoints).ToArray(); // Convert the hashset back to an array and return it
    }

}

