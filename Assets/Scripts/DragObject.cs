using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public int index;
    public ItemType itemType;
    public bool isWholeItem;

    private float mZCoord;
    private Vector3 mOffset;
    private float offsetedY;
    private bool offsetedYWas;

    private void Start()
    {
        if (isWholeItem)
            StartCoroutine(IESellItemAfterDelay());
    }

    private IEnumerator IESellItemAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        ItemSellBorder.Instance.SellItem(this);
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        if (isWholeItem)
            ItemSellBorder.Instance.MakeBorderGreen();
    }

    private void OnMouseUp()
    {
        if (isWholeItem)
            ItemSellBorder.Instance.MakeBorderWhite();

        offsetedYWas = false;
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
        if (!offsetedYWas)
        {
            offsetedY = transform.position.y + 0.5f;
            offsetedYWas = true;
        }
        transform.position = GetMouseAsWorldPoint() + mOffset;
        transform.position = new Vector3(transform.position.x, offsetedY, transform.position.z);
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