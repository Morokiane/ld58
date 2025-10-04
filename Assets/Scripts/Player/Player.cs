using UnityEngine;
using UnityEngine.InputSystem;

namespace  Player {
    public class Player : MonoBehaviour {
        public static Player instance;
        private static readonly int mining = Animator.StringToHash("mining");

        [SerializeField] private GameObject pick;
        public uint maxWeight = 50;

        private bool isMining;
        public uint currentWeight;

        private Animator anim;
        private Utils.Flash flash;
        private PlayerMovement playerMovement;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }

            anim = GetComponent<Animator>();
            flash = GetComponent<Utils.Flash>();
            playerMovement = GetComponent<PlayerMovement>();

            pick.SetActive(false);
        }

        public void Mining(InputAction.CallbackContext context) {
            if (context.performed && !isMining) {
                isMining = true;
                PlayerMovement.instance.canMove = false;
                anim.SetBool(mining, isMining);

                Rigidbody2D rb = PlayerMovement.instance.GetComponent<Rigidbody2D>();
                rb.linearVelocity = Vector2.zero;
            } else if (context.canceled) {
                isMining = false;
                PlayerMovement.instance.canMove = true;
                anim.SetBool(mining, isMining);
            }
        }

        public void DropOre(InputAction.CallbackContext context) {
            if (context.started && currentWeight > 0) {
                currentWeight--;
                RecalcWeight();
                Controllers.HUDController.instance.UpdateWeight();
            }
        }

        public void DamagePlayer(uint _damage) {
            if (currentWeight >= 1) {
                currentWeight -= _damage;
                Controllers.HUDController.instance.UpdateWeight();
                RecalcWeight();
            }
            
            StartCoroutine(flash.FlashRoutine());
            Utils.ScreenShake.instance.ShakeScreen();
        }

        public void RecalcWeight() {
            float weightPercent = currentWeight / 100f;
            playerMovement.moveSpeed = playerMovement.maxMoveSpeed - weightPercent;
            playerMovement.jumpPower = 5f - weightPercent;
            // Untested, but did have this happen. If it happens now with this, will have to put this somewhere else

            if (playerMovement.moveSpeed < 1) {
                playerMovement.moveSpeed = playerMovement.maxMoveSpeed;
            }
        }

        private void ResetMovement() {
            playerMovement.moveSpeed = 5f;
            playerMovement.jumpPower = 5f;
            playerMovement.maxJumps = 2;
            playerMovement.maxFallSpeed = 10f;
            playerMovement.fallSpeedMulti = 2f;
        }

        // These are called from the animator
        public void ActivatePick() {
            pick.SetActive(true);
        }

        public void DeactivatePick() {
            pick.SetActive(false);
        }
    }
}
