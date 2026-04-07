public interface IGameplayEvents
{
    void GhostEaten(Ghost ghost);
    void PacmanEaten();
    void PelletEaten(Pellets pellet);
    void PowerPelletEaten(PowerPellets pellet);
}