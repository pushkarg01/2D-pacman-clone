using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb {  get; private set; }

    public float speed = 8f;
    public float speedMultipiler = 1f;
    public Vector2 initialDirecction;
    public LayerMask obstacleLayer;

    public Vector2 direction {  get; private set; }
    public Vector2 nextDirection {  get; private set; }
    public Vector3 startDirection { get; private set; }


    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.startDirection = this.transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.speedMultipiler = 1f;
        this.direction = this.initialDirecction;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startDirection;
        this.rb.isKinematic = false;
        this.enabled = true;
    }

    private void Update()
    {
        if (this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = this.rb.position;
        Vector2 translation = this.direction * this.speed * Time.fixedDeltaTime;

        this.rb.MovePosition(position+translation);
    }

    public void SetDirection(Vector2 direction,bool forced =false)
    {
        if ( forced || !Occupied(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;

        }
        else
        {
            this.nextDirection =direction;
        } 
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f,this.obstacleLayer);

        return hit.collider != null;
    }
}
