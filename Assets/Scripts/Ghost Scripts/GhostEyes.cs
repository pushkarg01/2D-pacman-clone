using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    public SpriteRenderer spriteRenderer {  get; private set; }
    public Movement movement { get; private set; }

    public Sprite up,down,left,right;

    private void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        if(this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = this.up;
        }
        if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = this.down;
        }
        if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = this.left;
        }
        if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = this.right;
        }

    }
}
