using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AutoFocus : MonoBehaviour
{
    public DepthOfField dof;

	void Start ()
    {
        InvokeRepeating("RefocusUpdate", 0, 0.1f);		
	}

	void RefocusUpdate ()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            dof.focalLength = hit.distance;        
        }
	}
}
