using CameraController;
using Score;
using UnityEngine;

namespace Character
{
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
        private ScoreController score;
        public float scoringRatio;

        [Header("Game Over")] 
        public GameObject gameOverScreen;
        public float fallPositionY;

        [Header("Camera")]
        private CameraMoveController gameCamera;

        private Animator animator;
        private CharacterSoundController soundController;
        private Rigidbody2D rb2D;
    
        private bool isJumping,
            canJump,
            isOnGround;
        private float lastPositionX;
    

        private static readonly int IsOnGround = Animator.StringToHash("isOnGround");

        private void Start()
        {
            gameCamera = CameraMoveController.Instance;
            score = ScoreController.Instance;
            animator = GetComponent<Animator>();
            soundController = GetComponent<CharacterSoundController>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isOnGround && canJump)
                {
                    isJumping = true;
                    canJump = false;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            // If other is ground, ...
            if (((1 << other.gameObject.layer) & groundLayerMask) != 0)
            {
                // Set player is on ground and can jump
                isOnGround = true;
                canJump = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // If other is ground, ...
            if (((1 << other.gameObject.layer) & groundLayerMask) != 0)
            {
                // Set player is not on ground and can't jump
                isOnGround = false;
                canJump = false;
            }
        }

        private void FixedUpdate()
        {
            // Raycast ground
            // BUG: Raycast is only on the middle, so if player fall on the edge of foot, its animation can't be changed
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            //     Vector2.down, groundRaycastDistance, groundLayerMask);
        
            // if (!hit)
            // {
            //     isOnGround = false;
            //     // If player isn't on ground and velocity is 0,
            //     // isOnGround is true
            //     if (!isOnGround && rb2D.velocity.y <= 0)
            //         isOnGround = true;
            // }
            // else
            // {
            //     isOnGround = false;
            // }
        
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
        
            // Destroy character to prevent from falling
            Destroy(gameObject);
        }
    
        /// <summary>
        /// Draw line on editor
        /// </summary>
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Debug.DrawLine(position, 
                position + (Vector3.down* groundRaycastDistance), Color.white);
        }
    }
}
