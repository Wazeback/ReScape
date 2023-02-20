using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Object = UnityEngine.Object;


public class PickupController  : MonoBehaviour
{
    public Transform mainCam;
    public Image mainImage;
    public Image secondImage;
    
    private GameObject obj;
    private Rigidbody objRb;
    private GameObject pickObj;

    private int rayAmount = 30;
    private float[] distances = new float[30];

    private bool resizingPickObj;
    
    public float dist;

    private Vector3 direction;

    
    private void FixedUpdate()
    {
        HandleCursor();
        if (resizingPickObj)
            HandleResize();

    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (obj) 
                DropObj();
            else {
                RaycastHit hit;
                bool raycast = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, dist);
                if (raycast)
                    PickUpObject(hit.transform.gameObject);
            }
        }
    }

    private void PickUpObject(GameObject pickObj) 
    {
        if (pickObj.CompareTag("pickup")) {
            pickObj.layer = LayerMask.NameToLayer("WhatIsPickedUp");
            objRb = pickObj.GetComponent<Rigidbody>();
            objRb.useGravity = false;
            objRb.constraints = RigidbodyConstraints.FreezeAll;
            objRb.transform.parent = mainCam;
            obj = pickObj;
            resizingPickObj = true;
        }
    }
    
    private void DropObj()
    {
        resizingPickObj = false;
        objRb.transform.gameObject.layer = LayerMask.NameToLayer("Default");
        objRb.useGravity = true;
        objRb.constraints = RigidbodyConstraints.None;
        objRb.transform.parent = null;
        obj = null;
    }
    
    private Vector3[] GetObjectPoints(GameObject obj)
    {
        Vector3[] corners = new Vector3[rayAmount];
        Transform objPos = obj.transform;
        Vector3 center = objPos.position;
        Quaternion referenceRotation = objPos.rotation.normalized;
        Vector3 extents = obj.transform.lossyScale/2;
        
        //corners of the object
        for (UInt16 it = 0; it <= 7; it++)
            corners[it] = center + referenceRotation * new Vector3(extents.x*(1 - (4 & it)/2), extents.y*(1 - (2 & it)), extents.z*(1 - (1 & it)*2) );
        
        //center planes of tje object
        corners[8] = center + referenceRotation * new Vector3(extents.x, 0, 0);
        corners[9] = center + referenceRotation * new Vector3(-extents.x, 0, 0);
        corners[10] = center + referenceRotation * new Vector3(0, extents.y, 0);
        corners[11] = center + referenceRotation * new Vector3(0, -extents.y, 0);
        corners[12] = center + referenceRotation * new Vector3(0, 0, extents.z);
        corners[13] = center + referenceRotation * new Vector3(0, 0, -extents.z);

        //middele of every corner of the object.
        corners[14] = center + referenceRotation * new Vector3(extents.x, 0, extents.z);
        corners[15] = center + referenceRotation * new Vector3(-extents.x, 0, extents.z);
        corners[16] = center + referenceRotation * new Vector3(0, extents.y, extents.z);
        corners[17] = center + referenceRotation * new Vector3(0, -extents.y, extents.z);
        corners[18] = center + referenceRotation * new Vector3(extents.x, 0, -extents.z);
        corners[19] = center + referenceRotation * new Vector3(-extents.x, 0, -extents.z);
        corners[20] = center + referenceRotation * new Vector3(0, extents.y, -extents.z);
        corners[21] = center + referenceRotation * new Vector3(0, -extents.y, -extents.z);
        
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
    
    private void HandleCursor()
    {
        RaycastHit hitCheck;
        bool raycastCheck = Physics.Raycast(transform.position, mainCam.TransformDirection(Vector3.forward), out hitCheck, dist);
        if (raycastCheck && hitCheck.transform.CompareTag("pickup")) {
            mainImage.enabled = false; 
            secondImage.enabled = true;
        }
        else {
            mainImage.enabled = true;
            secondImage.enabled = false;
        }
    }

    private void HandleResize()
    {
        Vector3[] corners = GetObjectPoints(obj);
        float initialDistance = Vector3.Distance(mainCam.position, obj.transform.position);
            
        RaycastHit hit;
        int i = 0;
        foreach (Vector3 corner in corners) {
            direction = (corner - mainCam.position).normalized;
            if (Physics.Raycast(mainCam.position, direction, out hit, 500.0f, ~(1 << obj.layer))) {
                distances[i] = Vector3.Distance(mainCam.position, hit.point);
                i++; 
            }
            Debug.DrawLine(mainCam.position, corner + direction * 200.0f, Color.red, 0.1f);
        }
            
        float shortestValue = Mathf.Min(distances);
        obj.transform.position = 
            mainCam.position + mainCam.forward * 
            (shortestValue - Vector3.Distance(obj.transform.position, 
                corners[Array.IndexOf(distances, shortestValue)]));
            
        float scaleFactor = Vector3.Distance(mainCam.position, obj.transform.position) / initialDistance;
        obj.transform.localScale *= scaleFactor; 
        float mass = obj.GetComponent<Rigidbody>().mass;
        obj.GetComponent<Rigidbody>().mass = (mass * scaleFactor) * (mass * scaleFactor) * (mass * scaleFactor);
    }
    
}

