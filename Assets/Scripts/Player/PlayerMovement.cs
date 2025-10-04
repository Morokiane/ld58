using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerMovement : MonoBehaviour {
        public static PlayerMovement instance;

        // Animator Hashes
        private static readonly int magnitude = Animator.StringToHash("magnitude");
        private static readonly int jump = Animator.StringToHash("jump");
        private static readonly int yVelocity = Animator.StringToHash("yVelocity");
        private static readonly int wallSlide = Animator.StringToHash("wallSlide");
        private static readonly int grounded = Animator.StringToHash("grounded");

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpPower = 5f;
        [SerializeField] private int maxJumps = 2;

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
        [SerializeField] private float baseGravity = 2f;
        [SerializeField] private float maxFallSpeed = 18f;
        [SerializeField] private float fallSpeedMulti = 2f;

        public bool canMove = true;

        // Internal State
        private float horizontalMovement;
        private int jumpsRemaining;
        private bool isFacingRight = true;
        private bool isWallSliding;
        private bool isGrounded;
        private bool isWallJumping;
        private float wallJumpDirection;
        private float wallJumpTimer;

        private Rigidbody2D rb;
        private Animator anim;

        // Collider buffer for non-alloc overlap checks
        private readonly Collider2D[] hitBuffer = new Collider2D[1];

        private void Awake() {
            instance = this;
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Update() {
            // Handle animations only (lightweight)
            anim.SetFloat(magnitude, Mathf.Abs(rb.linearVelocity.x));
            anim.SetFloat(yVelocity, rb.linearVelocity.y);
            anim.SetBool(wallSlide, isWallSliding);
            anim.SetBool(grounded, isGrounded);
        }

        private void FixedUpdate() {
            GroundCheck();
            WallSlide();
            WallJump();
            ApplyGravity();

            if (!isWallJumping && canMove) {
                rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
                Flip();
            }
        }

        private void ApplyGravity() {
            if (rb.linearVelocity.y < 0) {
                rb.gravityScale = baseGravity * fallSpeedMulti;
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    Mathf.Max(rb.linearVelocity.y, -maxFallSpeed)
                );
            } else {
                rb.gravityScale = baseGravity;
            }
        }

        private void Flip() {
            if ((isFacingRight && horizontalMovement < 0) || (!isFacingRight && horizontalMovement > 0)) {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }
        }

        // Input callbacks
        public void Move(InputAction.CallbackContext context) {
            horizontalMovement = context.ReadValue<Vector2>().x;
        }

        public void Jump(InputAction.CallbackContext context) {
            if (context.performed) {
                if (jumpsRemaining > 0) {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                    jumpsRemaining--;
                    anim.SetTrigger(jump);
                }
                else if (wallJumpTimer > 0f) {
                    isWallJumping = true;
                    rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
                    wallJumpTimer = 0;
                    anim.SetTrigger(jump);

                    if (!Mathf.Approximately(transform.localScale.x, wallJumpDirection)) {
                        isFacingRight = !isFacingRight;
                        Vector3 ls = transform.localScale;
                        ls.x *= -1f;
                        transform.localScale = ls;
                    }

                    Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
                }
            }
            else if (context.canceled && rb.linearVelocity.y > 0) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }

        private void GroundCheck() {
        #pragma warning disable CS0618 // Type or member is obsolete
            int hits = Physics2D.OverlapBoxNonAlloc(
                groundCheckPos.position,
                groundCheckSize,
                0,
                hitBuffer,
                groundLayer
            );
        #pragma warning restore CS0618 // Type or member is obsolete
            isGrounded = hits > 0;
            if (isGrounded) jumpsRemaining = maxJumps;
        }

        private bool WallCheck() {
        #pragma warning disable CS0618 // Type or member is obsolete
            return Physics2D.OverlapBoxNonAlloc(
                wallCheckPos.position,
                wallCheckSize,
                0,
                hitBuffer,
                wallLayer
            ) > 0;
        #pragma warning restore CS0618 // Type or member is obsolete
        }

        private void WallSlide() {
            if (!isGrounded && WallCheck() && horizontalMovement != 0) {
                isWallSliding = true;
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed)
                );
            } else {
                isWallSliding = false;
            }
        }

        private void WallJump() {
            if (isWallSliding) {
                isWallJumping = false;
                wallJumpDirection = -Mathf.Sign(transform.localScale.x);
                wallJumpTimer = wallJumpTime;
                CancelInvoke(nameof(CancelWallJump));
            }
            else if (wallJumpTimer > 0f) {
                wallJumpTimer -= Time.fixedDeltaTime;
            }
        }

        private void CancelWallJump() {
            isWallJumping = false;
        }

        private void OnDrawGizmos() {
            if (groundCheckPos == null || wallCheckPos == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
        }
    }
}
