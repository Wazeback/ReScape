using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PickupController  : MonoBehaviour
{
    public Transform mainCam;
    public Image mainimage;
    public Image secondimage;
    
    private GameObject obj;
    private Rigidbody objRb;
    private GameObject pickObj;

    private bool resizingPickObj;
    
    public float dist;

    private float[] distances = new float[8];
    private Vector3[] closeHit = new Vector3[8];


    private Vector3 center;

    void FixedUpdate()
    {
        
        RaycastHit hitCheck;
        bool raycastCheck = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitCheck, dist);
        if (raycastCheck && hitCheck.transform.gameObject.CompareTag("pickup"))
        {
            mainimage.enabled = false;
            secondimage.enabled = true;
        }
        else
        {
            mainimage.enabled = true;
            secondimage.enabled = false;
            
        }
        
        if (resizingPickObj)
        {
            
            pickObj = obj;
            Vector3[] corners = GetObjectCorners(pickObj);
            
            float initialDistance = Vector3.Distance(mainCam.position, pickObj.transform.position);
            Vector3 direction = mainCam.forward;
            
            Bounds bounds = pickObj.GetComponent<Renderer>().bounds;
            float offset = (bounds.size.magnitude)/2;
            RaycastHit hit;

            int i = 0;
            foreach (Vector3 corner in corners)
            {
                direction = (corner - mainCam.position).normalized;
                {
                    if (Physics.Raycast(mainCam.position, direction, out hit, 200.0f, ~(1 << pickObj.layer)))
                    {
                        distances[i] = Vector3.Distance(mainCam.position, hit.point);
                        closeHit[i] = corner;
                        i++;
                    }

                    Debug.DrawLine(mainCam.position, corner + direction * 200.0f, Color.red, 0.1f);
                }
            }

            float shortestValue = Mathf.Min(distances);
            Vector3 closestPoint = closeHit[Array.IndexOf(distances, shortestValue)];
            

            if (shortestValue >= 0) 
            {
                pickObj.transform.position = mainCam.position + mainCam.forward * (shortestValue-Vector3.Distance(pickObj.transform.position, closestPoint));
                // pickObj.transform.position = pickObj.transform.position * (shortestValue - Vector3.Distance(pickObj.transform.position, closestPoint));
                // pickObj.transform.position = closestPoint;
                
                float scaleFactor = Vector3.Distance(mainCam.position, pickObj.transform.position) / initialDistance;
                pickObj.transform.localScale *= scaleFactor;  
            }

            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (obj == null)
            {
                RaycastHit hit;
                bool raycast = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, dist);
                if (raycast)
                {
                    PickUpObject(hit.transform.gameObject);
                }
                
            }
            else
            {
                dropObj();
            }
        }
        
    }

    void PickUpObject(GameObject pickObj)
    {
        if (pickObj.CompareTag("pickup"))
        {
            pickObj.layer = LayerMask.NameToLayer("WhatIsPickedUp");
            objRb = pickObj.GetComponent<Rigidbody>();
            objRb.useGravity = false;
            objRb.constraints = RigidbodyConstraints.FreezeAll;
            objRb.transform.parent = mainCam;
            obj = pickObj;

            resizingPickObj = true;
            
        }
    }
    
    void dropObj()
    {
        resizingPickObj = false;
        objRb.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        objRb.useGravity = true;
        objRb.constraints = RigidbodyConstraints.None;
        objRb.transform.parent = null;
        obj = null;
        
    }
    
    private Vector3[] GetObjectCorners(GameObject obj)
    {
        Vector3[] corners = new Vector3[8];
        Transform objPos = obj.GetComponent<Transform>();
        center = objPos.position;
        // center = obj.GetComponent<Renderer>().bounds.center;
        Quaternion referenceRotation = objPos.rotation.normalized;

        Debug.DrawLine(center, center+ new Vector3(0, 10, 0), Color.green);
        
        // Vector3 extents = obj.GetComponent<Renderer>().bounds.size/3f;

        Vector3 extents = obj.GetComponent<Transform>().lossyScale/2;
        // corners[0] = center + referenceRotation * new Vector3(extents.x, extents.y, extents.z);
        // corners[1] = center + referenceRotation * new Vector3(extents.x, extents.y, -extents.z);
        // corners[2] = center + referenceRotation * new Vector3(extents.x, -extents.y, extents.z);
        // corners[3] = center + referenceRotation * new Vector3(extents.x, -extents.y, -extents.z);
        // corners[4] = center + referenceRotation * new Vector3(-extents.x, extents.y, extents.z);
        // corners[5] = center + referenceRotation * new Vector3(-extents.x, extents.y, -extents.z);
        // corners[6] = center + referenceRotation * new Vector3(-extents.x, -extents.y, extents.z);
        // corners[7] = center + referenceRotation * new Vector3(-extents.x, -extents.y, -extents.z);
        for (UInt16 it = 0; it < 8; it++)
        {
            corners[it] = center + referenceRotation * new Vector3(extents.x*(1 - (4 & it)/2), extents.y*(1 - (2 & it)), extents.z*(1 - (1 & it)*2) );
            Debug.DrawLine(center, corners[it], Color.blue);
        }
        return corners;
    }
    
    


}