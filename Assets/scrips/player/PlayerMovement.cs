using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // movement speed
    public float maxSpeed = 20f; // maximum speed
    public float jumpForce = 5f; // jump force
    public float groundCheckDistance = 1f; // distance from the ground to check for jump
    public LayerMask groundLayer; // layer for the ground objects

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = 0.5f; // increase drag to reduce sliding
        rb.mass = 1.5f; // increase mass to make player less prone to being pushed around
    }

    void FixedUpdate()
    {
        // check if player is on the ground
        bool isGrounded = Physics.Raycast(transform.position, -Vector3.up, groundCheckDistance, groundLayer);

        // get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // calculate movement direction
        Vector3 moveDirection = (moveHorizontal * transform.right + moveVertical * transform.forward).normalized;

        // apply movement force
        if (moveDirection.magnitude > 0)
        {
            rb.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);

            // clamp velocity to maximum speed
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
            // set velocity to zero when there is no input
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        // check if player wants to jump and is on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}