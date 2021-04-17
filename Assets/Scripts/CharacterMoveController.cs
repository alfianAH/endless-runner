using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveAcceleration;
    public float maxSpeed;

    [Header("Jump")] 
    public float jumpAcceleration;

    [Header("Ground Raycast")] 
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;
    
    private Rigidbody2D rb2D;
    private Animator animator;
    
    private bool isJumping,
        isOnGround;

    private static readonly int IsOnGround = Animator.StringToHash("isOnGround");

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
                isJumping = true;
        }
        
        // Set jump animation
        animator.SetBool(IsOnGround, isOnGround);
    }

    private void FixedUpdate()
    {
        // Raycast ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            Vector2.down, groundRaycastDistance, groundLayerMask);
        
        if (hit)
        {
            // If player isn't on ground and velocity is 0,
            // isOnGround is true
            if (!isOnGround && rb2D.velocity.y <= 0)
                isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
        
        Vector2 velocityVector = rb2D.velocity;
        velocityVector.x = Mathf.Clamp(
            velocityVector.x + moveAcceleration * Time.deltaTime, 
            0f, maxSpeed);
        
        // If player click the mouse, jump
        if (isJumping)
        {
            velocityVector.y += jumpAcceleration;
            isJumping = false;
        }
        
        rb2D.velocity = velocityVector;
    }
    
    /// <summary>
    /// Draw line on editor
    /// </summary>
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down* groundRaycastDistance), Color.white);
    }
}
