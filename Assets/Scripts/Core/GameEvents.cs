using System;

public class GameEvents
{
    public event Action<Pellets> PelletEaten;
    public void RaisePelletEaten(Pellets pellet) => PelletEaten?.Invoke(pellet);


    public event Action<PowerPellets> PowerPelletEaten;
    public void RaisePowerPelletEaten(PowerPellets pellet) => PowerPelletEaten?.Invoke(pellet);


    public event Action<Ghost> GhostEaten;
    public void RaiseGhostEaten(Ghost ghost) => GhostEaten?.Invoke(ghost);


    public event Action PacmanEaten;
    public void RaisePacmanEaten() => PacmanEaten?.Invoke();


    public event Action<int> ScoreChanged;
    public void RaiseScoreChanged(int score) => ScoreChanged?.Invoke(score);


    public event Action<int> LivesChanged;
    public void RaiseLivesChanged(int lives) => LivesChanged?.Invoke(lives);


    public event Action<bool> GameOverChanged;
    public void RaiseGameOverChanged(bool isGameOver) => GameOverChanged?.Invoke(isGameOver);
}
