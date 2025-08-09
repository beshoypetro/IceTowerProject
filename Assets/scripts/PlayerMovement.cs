using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private float _horizontalInput;
    [SerializeField] float pushForce = 500f;
    [SerializeField] float linearDamping = 5f;
    [SerializeField] float maxVelocity = 40f;
    [SerializeField] float jumpPower = 30f;
    [SerializeField] float jumpPowerMultiplier = 1.1f;
    private bool _isGrounded = false;
    [SerializeField] private float groundCheckDistance = 1.4f;
     private LayerMask _groundLayer;
     private SoundManager _soundManager;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _soundManager = GetComponent<SoundManager>();
        _groundLayer = LayerMask.GetMask("Ground");

        rb.freezeRotation = true;
        rb.linearDamping = linearDamping;
    }

    void Update()
    {
        CheckIsGrounded();
        

        
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            float speedFactor = Mathf.Abs(rb.linearVelocity.x);
            float dynamicJumpPower = jumpPower + speedFactor * jumpPowerMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, dynamicJumpPower);
            animator.SetBool("isJumping", !_isGrounded);
            _soundManager.Jump();
        }
        
        _horizontalInput = Input.GetAxis("Horizontal");
        
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
        
        if (_horizontalInput != 0)
        {
            rb.AddForce(new Vector2(_horizontalInput * pushForce, 0), ForceMode2D.Force);
        }

     
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
           
            Vector2 vel = rb.linearVelocity;

            // Clamp only horizontal movement, leave vertical untouched
            if (Mathf.Abs(vel.x) > maxVelocity)
            {
                vel.x = Mathf.Sign(vel.x) * maxVelocity;
                rb.linearVelocity = new Vector2(vel.x, vel.y);
            }
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
        animator.SetBool("isJumping", !_isGrounded);
        if (collision.gameObject.CompareTag("Platform"))
        {
            ScoreManager.Instance.PlayerLandedOnPlatform(collision.transform.position.y);
        }
    }

    private void CheckIsGrounded()
    {
        _isGrounded = true;
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, _groundLayer);
        // _isGrounded = hit.collider != null;
    }


}