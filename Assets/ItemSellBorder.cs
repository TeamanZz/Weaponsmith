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
            Destroy(other.gameObject);
        }
    }
}
