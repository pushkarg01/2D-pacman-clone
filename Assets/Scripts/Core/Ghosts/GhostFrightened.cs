using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehaviour
{
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    public bool eaten {  get; private set; }

    private void OnEnable()
    {
        ghost.move.speedMultipiler = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        ghost.move.speedMultipiler = 1f;
        eaten = false;
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke("Flash", duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        white.enabled = false;
        blue.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;

            white.GetComponent<AnimatedSprites>().RestartAnim();
        }
    }

    private void Eaten()
    {
        eaten =true;

        Vector3 position = ghost.home.inside.position;
        position.z = ghost.transform.position.z;
        ghost.transform.position = position;

        ghost.home.Enable(duration);

        body.enabled = false;
        eyes.enabled = true;
        white.enabled = false;
        blue.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();
            }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDiection in node.availableDirection)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDiection.x, availableDiection.y, 0f);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDiection;
                    maxDistance = distance;
                }
            }
            ghost.move.SetDirection(direction);
        }
    }
}
