using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFromTheDungeon : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && other.GetComponent<Character>())
        {
            DungeonGameManager.dungeonGameManager.UploadNewPiece();
            Debug.Log("Exit");
        }
    }
}
