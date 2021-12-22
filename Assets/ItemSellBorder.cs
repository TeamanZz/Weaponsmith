using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSellBorder : MonoBehaviour
{
    public GameObject destroyParticles;
    private void OnTriggerEnter(Collider other)
    {
        DragObject dragObject;
        if (other.TryGetComponent<DragObject>(out dragObject))
        {
            if (!dragObject.isWholeItem)
                return;
            var newParticles = Instantiate(destroyParticles, dragObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
            Destroy(other.gameObject);
        }
    }
}