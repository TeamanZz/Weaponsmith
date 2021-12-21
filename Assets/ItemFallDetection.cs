using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DragObject dragObject;
        if (other.TryGetComponent<DragObject>(out dragObject))
        {
            dragObject.transform.position = new Vector3(Random.Range(-2.4f, 1.6f), 4.3f, Random.Range(-0.021f, -2.279f));
        }
    }
}