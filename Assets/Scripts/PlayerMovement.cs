using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    public Animator animator;
    public Transform sprite;
    public AudioSource audioSource; 
    public AudioClip walkingSound;

    private bool facingRight = true;
    private bool canMove = true;

    public void SetMovement(bool state)
    {
        canMove = state;
        if (!canMove)
        {
            body.velocity = Vector2.zero;
            StopWalkingSound();
        }
    }

    void Update()
    {
        if (!canMove) return;

        float xInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(xInput));

        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * speed, body.velocity.y);
            PlayWalkingSound(); 

            if (xInput > 0 && !facingRight)
            {
                Flip();
            }
            else if (xInput < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            body.velocity = new Vector2(0f, body.velocity.y);
            StopWalkingSound();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 localScale = sprite.localScale;
        localScale.x *= -1;
        sprite.localScale = localScale;
    }

    private void PlayWalkingSound()
    {
        if (audioSource != null && walkingSound != null && !audioSource.isPlaying)
        {
            audioSource.clip = walkingSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopWalkingSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
