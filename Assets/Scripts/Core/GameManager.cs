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
        if (gameEvents == null) return;

        gameEvents.GhostEaten += OnGhostEaten;
        gameEvents.PacmanEaten += OnPacmanEaten;
        gameEvents.PelletEaten += OnPelletEaten;
        gameEvents.PowerPelletEaten += OnPowerPelletEaten;
    }

    private void UnsubscribeEvents()
    {
        if (gameEvents == null) return;

        gameEvents.GhostEaten -= OnGhostEaten;
        gameEvents.PacmanEaten -= OnPacmanEaten;
        gameEvents.PelletEaten -= OnPelletEaten;
        gameEvents.PowerPelletEaten -= OnPowerPelletEaten;
    }

    public void StartNewGame()
    {
        isGameOver = false;
        gameEvents.RaiseGameOverChanged(isGameOver);

        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);

        isGameOver = true;
        gameEvents.RaiseGameOverChanged(isGameOver);
    }

    private void SetScore(int value)
    {
        score = value;
        gameEvents.RaiseScoreChanged(score);
    }

    private void SetLives(int value)
    {
        lives = value;
        gameEvents.RaiseLivesChanged(lives);
    }

    private void OnGhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);
        ghostMultiplier++;
    }

    private void OnPacmanEaten()
    {
        pacman.gameObject.SetActive(false);
        SetLives(lives - 1);

        if (lives > 0)
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
        SetScore(score + pellet.points);

        if (!HasRemainingPellet())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void OnPowerPelletEaten(PowerPellets pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        OnPelletEaten(pellet);

        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellet()
    {
        foreach (Transform pellet in pellets)
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
        ghostMultiplier = 1;
    }
}
