using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehaviour
{
    public Transform inside , outside;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            this.ghost.move.SetDirection(-this.ghost.move.direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        this.ghost.move.SetDirection(Vector2.up,true);
        this.ghost.move.rb.isKinematic = true;
        this.ghost.move.enabled = false;

        Vector3 position = this.transform.position;
        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPos = Vector3.Lerp(position, this.inside.position,elapsed /duration);
            newPos.z = position.z;
            this.ghost.transform.position = newPos;
            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPos = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
            newPos.z = position.z;
            this.ghost.transform.position = newPos;
            elapsed += Time.deltaTime;

            yield return null;
        }

        this.ghost.move.SetDirection(new Vector2(Random.value < 0.5f ? -1f:1f,0),true);
        this.ghost.move.rb.isKinematic = false;
        this.ghost.move.enabled = true;
    }

    
}
