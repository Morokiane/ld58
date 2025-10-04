using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovement : MonoBehaviour {
        public static PlayerMovement instance;
        private static readonly int magnitude = Animator.StringToHash("magnitude");
        private static readonly int jump = Animator.StringToHash("jump");
        private static readonly int yVelocity = Animator.StringToHash("yVelocity");
        private static readonly int wallSlide = Animator.StringToHash("wallSlide");
        private static readonly int grounded = Animator.StringToHash("grounded");

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

        public bool canMove = true;

        private float horizontalMovement;
        private int jumpsRemaining;
        private bool isFacingRight = true;
        private bool isWallSliding;
        private bool isGrounded;
        private bool isWallJumping;
        private float wallJumpDirection;
        private float wallJumpTimer;
        private bool isMining;
        
        private Rigidbody2D rb;
        private Animator anim;

        private void Awake() {
            instance = this;

            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update() {
            float speed = Mathf.Abs(rb.linearVelocity.x);
            anim.SetFloat(magnitude, speed);
            anim.SetFloat(yVelocity, rb.linearVelocity.y);
            anim.SetBool(wallSlide, isWallSliding);
            anim.SetBool(grounded, isGrounded);
        }


        private void FixedUpdate() {
            GroundCheck();
            Gravity();
            WallSlide();
            WallJump();

            if (!isWallJumping && canMove) {
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
                anim.SetTrigger(jump); // only once, on performed
            }
            else if (context.canceled && rb.linearVelocity.y > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }

            // Wall jump logic stays the same, just don’t trigger extra anims
            if (context.performed && wallJumpTimer > 0f) {
                isWallJumping = true;
                rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                wallJumpTimer = 0;
                anim.SetTrigger(jump);

                if (!Mathf.Approximately(transform.localScale.x, wallJumpDirection)) {
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
