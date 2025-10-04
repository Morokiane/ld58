using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovement : MonoBehaviour {

        [SerializeField] private float moveSpeed = 5f;
        [Header("Jump Settings")]
        [SerializeField] private float jumpPower = 5f;
        [SerializeField] int maxJumps = 2;
        
        [Header("Ground Check")]
        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Wall Check")]
        [SerializeField] private Transform wallCheckPos;
        [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
        [SerializeField] private LayerMask wallLayer;

        [Header("Wall Movement")]
        [SerializeField] private float wallSlideSpeed = 2f;
        [SerializeField] private float wallJumpTime = 0.5f;
        [SerializeField] private Vector2 wallJumpPower = new Vector2(5f, 10f);

        [Header("Gravity Settings")]
        [SerializeField] private float baseGravity = 2;
        [SerializeField] private float maxFallSpeed = 18f;
        [SerializeField] private float fallSpeedMulti = 2f;
        
        private float horizontalMovement;
        private int jumpsRemaining;
        private bool isFacingRight = true;
        private bool isWallSliding;
        private bool isGrounded;
        private bool isWallJumping;
        private float wallJumpDirection;
        private float wallJumpTimer;
        
        private Rigidbody2D rb;
        
        private void Start() {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
        
        }

        private void FixedUpdate() {
            GroundCheck();
            Gravity();
            WallSlide();
            WallJump();

            if (!isWallJumping) {
                rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
                Flip();
            }
        }

        // As you collect more stuff increase the gravity
        private void Gravity() {
            if (rb.linearVelocity.y < 0) {
                rb.gravityScale = baseGravity * fallSpeedMulti;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
            } else {
                rb.gravityScale = baseGravity;
            }
        }

        private void Flip() {
            if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) {
                isFacingRight = !isFacingRight;
                Vector2 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }

        public void Move(InputAction.CallbackContext context) {
            horizontalMovement = context.ReadValue<Vector2>().x;
        }

        public void Jump(InputAction.CallbackContext context) {
            if (context.performed && jumpsRemaining > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
            } else if (context.canceled) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
            }

            if (context.performed && wallJumpTimer > 0f) {
                isWallJumping = true;
                rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = 0;

                if (transform.localScale.x != wallJumpDirection) {
                    isFacingRight = !isFacingRight;
                    Vector2 ls = transform.localScale;
                    ls.x *= -1f;
                    transform.localScale = ls;
                }

                Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            }
        }

        private void GroundCheck() {
            if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)) {
                jumpsRemaining = maxJumps;
                isGrounded = true;
            } else {
                isGrounded = false;
            }
        }

        private bool WallCheck() {
            return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer); 
        }

        private void WallSlide() {
            if (!isGrounded && WallCheck() && horizontalMovement != 0) {
                isWallSliding = true;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
            } else {
                isWallSliding = false;
            }
        }

        private void WallJump() {
            if (isWallSliding) {
                isWallJumping = false;
                wallJumpDirection = -transform.localScale.x;
                wallJumpTimer = wallJumpTime;

                CancelInvoke(nameof(CancelWallJump));
            } else if (wallJumpTimer > 0f) {
                wallJumpTimer -= Time.deltaTime;
            }
        }

        private void CancelWallJump() {
            isWallJumping = false;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        }
    }
}
