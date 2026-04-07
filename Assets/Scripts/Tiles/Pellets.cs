using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Pellets : MonoBehaviour
{
    public int points = 10;

    protected IGameplayEvents gameplayEvents;

    [Inject]
    private void Construct(IGameplayEvents gameplayEvents)
    {
        this.gameplayEvents = gameplayEvents;
    }

    protected virtual void Eat()
    {
        this.gameplayEvents.PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
