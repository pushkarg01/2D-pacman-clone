using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PowerPellets : Pellets
{
    public float duration = 8.0f;

    protected override void Eat()
    {
        gameEvents?.RaisePowerPelletEaten(this);
    }
}
