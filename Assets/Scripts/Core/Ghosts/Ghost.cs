using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ghost : MonoBehaviour
{
    public Movement move { get; private set; }
    public GhostHome home { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostScatter scatter { get; private set; }

    public GhostBehaviour initialBehaviour;
    public Transform target;
    public int points = 200;

    private GameEvents gameEvents;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    private void Awake()
    {
        move = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        move.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehaviour)
        {
            home.Disable();
        }

        if (initialBehaviour != null)
        {
            initialBehaviour.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Pacman"))
        {
            return;
        }

        if (gameEvents == null)
        {
            return;
        }

        if (frightened.enabled)
        {
            gameEvents.RaiseGhostEaten(this);
        }
        else
        {
            gameEvents.RaisePacmanEaten();
        }
    }
}

