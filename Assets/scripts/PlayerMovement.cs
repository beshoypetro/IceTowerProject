using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] float pushForce = 100f;
    [SerializeField] float linearDamping = 0.95f;
    [SerializeField] float maxVelocity = 5f;
    [SerializeField] float jumpPower = 20f;
    [SerializeField] float jumpPowerMultiplier = 1.1f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        rb.linearDamping = linearDamping;
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            float speedFactor = Mathf.Abs(rb.linearVelocity.x);
            float dynamicJumpPower = jumpPower + speedFactor * jumpPowerMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, dynamicJumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
        
        horizontalInput = Input.GetAxis("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FaceLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FaceRight();
        }
    }

    private void FixedUpdate()
    {
        
        if (horizontalInput != 0)
        {
            rb.AddForce(new Vector2(horizontalInput * pushForce, 0), ForceMode2D.Force);
        }

     
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
           
            rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
        }

        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void FaceLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    
    private void FaceRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
        if (collision.gameObject.CompareTag("Platform"))
        {
            ScoreManager.Instance.PlayerLandedOnPlatform(collision.transform.position.y);
        }
    }
    
    
}