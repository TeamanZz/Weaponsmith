using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;

    private float mZCoord;

    private float yPos = 2;
    public int index;
    public ItemType itemType;
    public bool isWholeItem;

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {
        DragObject dragObject;
        if (other.gameObject.TryGetComponent<DragObject>(out dragObject))
        {
            if (isWholeItem || dragObject.isWholeItem)
                return;

            if (CraftManager.Instance.isSpawned)
                return;

            if (dragObject.index != index)
                return;

            if (dragObject.itemType == itemType)
                return;

            CraftManager.Instance.SpawnNewItem(index, transform.position, transform.rotation);
            Destroy(gameObject);
            if (other.gameObject != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}