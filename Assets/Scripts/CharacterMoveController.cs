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

    [Header("Scoring")] 
    public ScoreController score;
    public float scoringRatio;

    [Header("Game Over")] 
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")] 
    public CameraMoveController gameCamera;

    private Animator animator;
    private CharacterSoundController soundController;
    private Rigidbody2D rb2D;
    
    private bool isJumping,
        isOnGround;
    private float lastPositionX;
    

    private static readonly int IsOnGround = Animator.StringToHash("isOnGround");

    private void Start()
    {
        animator = GetComponent<Animator>();
        soundController = GetComponent<CharacterSoundController>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isOnGround)
            {
                isJumping = true;
                soundController.PlayJump();
            }
        }
        
        // Set jump animation
        animator.SetBool(IsOnGround, isOnGround);
        
        // Calculate Score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);
        
        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }

        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }
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
        
        Debug.Log("isOnGround: " + isOnGround);
        
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
    /// Game over
    /// </summary>
    private void GameOver()
    {
        // Set high score
        score.FinishScoring();
        
        // Stop camera movement
        gameCamera.enabled = false;
        
        // Show game over
        gameOverScreen.SetActive(true);
        
        // Disable character movement
        this.enabled = false;
    }
    
    /// <summary>
    /// Draw line on editor
    /// </summary>
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down* groundRaycastDistance), Color.white);
    }
}
