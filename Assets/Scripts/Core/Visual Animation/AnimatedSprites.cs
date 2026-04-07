using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprites : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    public float animationTime = 0.25f;
    public int animationFrame {  get; private set; }

    public bool isLoop = true;

    private void Awake()
    {
       spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating("Advance",animationTime,animationTime);
    }

    private void Advance()
    {
        if(!spriteRenderer.enabled) return;

        animationFrame++;
        if(animationFrame >= sprites.Length  && isLoop) { animationFrame = 0; }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void RestartAnim()
    {
        animationFrame = -1;

        Advance();
    }

}
