using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    public Ghost[] ghots;
    public Pacman pacman;
    public Transform pellets;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    public int score { get; private set; }
    public int lives { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;
  #endregion

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (this.lives <=0 && Input.anyKeyDown)
        {
            NewGame();
        }
    }

    void NewGame()
    {
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
        for (int i = 0; i < this.ghots.Length; i++)
        {
            this.ghots[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        for(int i = 0; i < this.ghots.Length; i++)
        {
            this.ghots[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2,'0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text ="x"+lives.ToString();
    }

    public void GhostEaten(Ghost ghost)
    {
        int points =ghost.points *this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.gameObject.SetActive(false );

        SetLives(this.lives -1);

        if(this.lives > 0)
        {
           Invoke("ResetState",3f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellets pallet)
    {
        pallet.gameObject.SetActive(false );

        SetScore(this.score + pallet.points);

        if (!HasRemainingPallet())
        {
            this.pacman.gameObject.SetActive(false ) ;

            Invoke("NewRound", 3f);
        }
    }

    public void PowerPelletEaten(PowerPellets pallet)
    {
        for (int i = 0; i < this.ghots.Length; i++)
        {
            this.ghots[i].frightened.Enable(pallet.duration);
        }

        PelletEaten(pallet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pallet.duration);
    }

    private bool HasRemainingPallet()
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
        this .ghostMultiplier = 1;
    }
}
