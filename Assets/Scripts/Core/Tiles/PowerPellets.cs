using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PowerPellets : Pellets
{
    public float duration = 8.0f;

    private GameEvents gameEvents;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    protected override void Eat()
    {
        if (gameEvents == null) return;
        gameEvents.RaisePowerPelletEaten(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
