using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehaviour
{
    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if(node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            int index = Random.Range(0,node.availableDirection.Count);
            if (node.availableDirection[index] == -this.ghost.move.direction && node.availableDirection.Count >1)
            {
                index++;

                if(index >= node.availableDirection.Count)
                {
                    index = 0;
                }
            }
            this.ghost.move.SetDirection(node.availableDirection[index]);
        }
    }
}
