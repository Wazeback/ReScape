using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class breakscript : MonoBehaviour
{
    private Rigidbody RB;
    private bool gothit = true;
    private float dist = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (gothit)
        {
            if (collision.rigidbody.mass >= 100)
            {
                RB = GetComponent<Rigidbody>(); 
                RB.isKinematic = false;
                gothit = false;
            }
        }
        
    }

    private void FixedUpdate()
    {
        
        if (gothit)
        {
            RaycastHit hit;
            if (!Physics.Raycast( transform.position ,    transform.forward,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   -transform.forward,  out hit, dist)  ||
                
                !Physics.Raycast(transform.position ,  transform.right,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   -transform.right,  out hit, dist)  ||
                
                !Physics.Raycast( transform.position ,    transform.forward,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   -transform.right,  out hit, dist)  ||
                
                !Physics.Raycast(transform.position ,  transform.right,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   -transform.forward,  out hit, dist)  ||
                
                !Physics.Raycast( transform.position ,    transform.forward,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   transform.right,  out hit, dist)  ||
                
                !Physics.Raycast(transform.position ,  transform.right,  out hit, dist) &&
                !Physics.Raycast(transform.position ,   transform.forward,  out hit, dist)  
                

               )
            {
                RB = GetComponent<Rigidbody>(); 
                RB.isKinematic = false;
                gothit = false;
            }
        }
    }
}
