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
       this.spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating("Advance",this.animationTime,this.animationTime);
    }

    private void Advance()
    {
        if(!this.spriteRenderer.enabled) return;

        this.animationFrame++;
        if(this.animationFrame >= this.sprites.Length  && this.isLoop) { this.animationFrame = 0; }

        if (this.animationFrame >= 0 && this.animationFrame < this.sprites.Length)
        {
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    public void RestartAnim()
    {
        this.animationFrame = -1;

        Advance();
    }

}
