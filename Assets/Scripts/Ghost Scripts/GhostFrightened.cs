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
        this.ghost.move.speedMultipiler = 0.5f;
        this.eaten = false;
    }

    private void OnDisable()
    {
        this.ghost.move.speedMultipiler = 1f;
        this.eaten = false;
    }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.eyes.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        Invoke("Flash", duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;
        this.eyes.enabled = true;
        this.white.enabled = false;
        this.blue.enabled = false;
    }

    private void Flash()
    {
        if (!this.eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;

            this.white.GetComponent<AnimatedSprites>().RestartAnim();
        }
    }

    private void Eaten()
    {
        this.eaten =true;

        Vector3 position = this.ghost.home.inside.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        this.ghost.home.Enable(this.duration);

        this.body.enabled = false;
        this.eyes.enabled = true;
        this.white.enabled = false;
        this.blue.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                Eaten();
            }
           
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDiection in node.availableDirection)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDiection.x, availableDiection.y, 0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDiection;
                    maxDistance = distance;
                }
            }
            this.ghost.move.SetDirection(direction);
        }
    }
}
