using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }

    public float speed = 8f;
    public float speedMultipiler = 1f;
    public Vector2 initialDirecction;
    public LayerMask obstacleLayer;

    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startDirection { get; private set; }

    private GameEvents gameEvents;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startDirection = transform.position;
    }

    private void OnEnable()
    {
        if (gameEvents != null)
        {
            gameEvents.GameOverChanged += OnGameOverChanged;
        }
    }

    private void Start()
    {
        ResetState();
    }

    private void OnDisable()
    {
        if (gameEvents != null)
        {
            gameEvents.GameOverChanged -= OnGameOverChanged;
        }
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
        Vector2 translation = direction * speed * speedMultipiler * Time.fixedDeltaTime;
        rb.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position,
            Vector2.one * 0.75f,
            0.0f,
            direction,
            1.5f,
            obstacleLayer);

        return hit.collider != null;
    }

    private void OnGameOverChanged(bool isGameOver)
    {
        if (rb != null)
        {
            rb.isKinematic = isGameOver;
        }

        if (isGameOver)
        {
            nextDirection = Vector2.zero;
            direction = Vector2.zero;
        }
    }
}
