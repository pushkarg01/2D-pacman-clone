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
        rb = GetComponent<Rigidbody2D>();
        startDirection = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        speedMultipiler = 1f;
        direction = initialDirecction;
        nextDirection = Vector2.zero;
        transform.position = startDirection;
        rb.isKinematic = false;
        enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(position+translation);
    }

    public void SetDirection(Vector2 direction,bool forced =false)
    {
        if ( forced || !Occupied(direction))
        {
            direction = direction;
            nextDirection = Vector2.zero;

        }
        else
        {
            nextDirection =direction;
        } 
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f,obstacleLayer);

        return hit.collider != null;
    }
}
