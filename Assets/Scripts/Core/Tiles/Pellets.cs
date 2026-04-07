using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pellets : MonoBehaviour
{
    public int points = 10;

    protected GameEvents gameEvents;

    [Inject]
    private void Construct(GameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    protected virtual void Eat()
    {
        if (gameEvents == null) return;
        gameEvents.RaisePelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
