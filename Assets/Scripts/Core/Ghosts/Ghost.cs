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

    private IGameplayEvents gameplayEvents;

    [Inject]
    private void Construct(IGameplayEvents gameplayEvents)
    {
        this.gameplayEvents = gameplayEvents;
    }

    private void Awake()
    {
        this.move = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();  
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.move.ResetState();

        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        if (this.home != this.initialBehaviour)
        {
            this.home.Disable();
        }
        
        if(this.initialBehaviour != null)
        {
            this.initialBehaviour.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.frightened.enabled)
            {
                this.gameplayEvents.GhostEaten(this);
            }
            else
            {
                this.gameplayEvents.PacmanEaten();
            }
        }
    }
}

