using System;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;
    public bool isGameOver { get; private set; }

    private GameEvents gameEvents;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    #endregion

    private void Start()
    {
        SubscribeEvents();
        StartNewGame();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        if (this.gameEvents == null) return;

        this.gameEvents.GhostEaten += OnGhostEaten;
        this.gameEvents.PacmanEaten += OnPacmanEaten;
        this.gameEvents.PelletEaten += OnPelletEaten;
        this.gameEvents.PowerPelletEaten += OnPowerPelletEaten;
    }

    private void UnsubscribeEvents()
    {
        if (this.gameEvents == null) return;

        this.gameEvents.GhostEaten -= OnGhostEaten;
        this.gameEvents.PacmanEaten -= OnPacmanEaten;
        this.gameEvents.PelletEaten -= OnPelletEaten;
        this.gameEvents.PowerPelletEaten -= OnPowerPelletEaten;
    }

    public void StartNewGame()
    {
        this.isGameOver = false;
        this.gameEvents.RaiseGameOverChanged(this.isGameOver);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);

        this.isGameOver = true;
        this.gameEvents.RaiseGameOverChanged(this.isGameOver);
    }

    private void SetScore(int value)
    {
        this.score = value;
        this.gameEvents.RaiseScoreChanged(this.score);
    }

    private void SetLives(int value)
    {
        this.lives = value;
        this.gameEvents.RaiseLivesChanged(this.lives);
    }

    private void OnGhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    private void OnPacmanEaten()
    {
        this.pacman.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            GameOver();
        }
    }

    private void OnPelletEaten(Pellets pellet)
    {
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        if (!HasRemainingPellet())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void OnPowerPelletEaten(PowerPellets pellet)
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        OnPelletEaten(pellet);

        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellet()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
