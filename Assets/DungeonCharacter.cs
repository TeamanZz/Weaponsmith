using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCharacter : MonoBehaviour
{
    public float runSpeed;
    private void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, runSpeed);
    }
}
