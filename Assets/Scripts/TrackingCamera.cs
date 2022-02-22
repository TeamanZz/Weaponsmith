using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
    public Transform target;
    
    [Range(0f, 100f)]
    public float targetDistance = 5f;
    public void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, target.position.z - targetDistance);    
    }
}
