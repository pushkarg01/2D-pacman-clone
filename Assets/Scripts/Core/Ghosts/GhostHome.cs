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
        if (gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.move.SetDirection(-ghost.move.direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        ghost.move.SetDirection(Vector2.up,true);
        ghost.move.rb.isKinematic = true;
        ghost.move.enabled = false;

        Vector3 position = transform.position;
        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPos = Vector3.Lerp(position, inside.position,elapsed /duration);
            newPos.z = position.z;
            ghost.transform.position = newPos;
            elapsed += Time.deltaTime;

            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPos = Vector3.Lerp(inside.position, outside.position, elapsed / duration);
            newPos.z = position.z;
            ghost.transform.position = newPos;
            elapsed += Time.deltaTime;

            yield return null;
        }

        ghost.move.SetDirection(new Vector2(Random.value < 0.5f ? -1f:1f,0),true);
        ghost.move.rb.isKinematic = false;
        ghost.move.enabled = true;
    }

    
}
