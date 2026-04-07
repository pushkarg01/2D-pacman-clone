using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 pos = other.transform.position;
        pos.x = connection.position.x;
        pos.y = connection.position.y;

        other.transform.position = pos;
    }
}
