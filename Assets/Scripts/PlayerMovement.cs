using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    public Animator animator;
    public Transform sprite; // Reference to the sprite transform

    private bool facingRight = true;
    private bool canMove = true; // Flag to control movement

    // Public method to toggle movement (called from PlayerInteraction)
    public void SetMovement(bool state)
    {
        canMove = state;
        if (!canMove)
        {
            body.velocity = Vector2.zero; // Stop movement immediately when disabled
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow movement if canMove is true
        if (!canMove) return;

        float xInput = Input.GetAxis("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(xInput));

        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * speed, body.velocity.y);

            // Check the direction and flip the sprite
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
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        // Flip the sprite by inverting its local scale on the X-axis
        Vector3 localScale = sprite.localScale;
        localScale.x *= -1;
        sprite.localScale = localScale;
    }
}
