using UnityEngine;
using UnityEngine.InputSystem;

namespace  Player {
    public class Player : MonoBehaviour {
        public static Player instance;
        private static readonly int mining = Animator.StringToHash("mining");

        [SerializeField] private GameObject pick;
        public uint maxWeight = 50;
        public bool canMove = true;
        [Header("SFX")]
        [SerializeField] private AudioClip tinkSFX;
        [SerializeField] private AudioClip hurtSFX;
        [SerializeField] private AudioClip trashSFX;

        [SerializeField] private GameObject droppedItem;

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
                DeactivatePick(); // Just is case it fucks up
                anim.SetBool(mining, isMining);
            }
        }

        public void DropOre(InputAction.CallbackContext context) {
            if (context.started && currentWeight > 0) {
                currentWeight--;
                AudioSource.PlayClipAtPoint(trashSFX, transform.position, 2f);
                RecalcWeight();
                Controllers.GameController.instance.amountTrashed++;
                Controllers.HUDController.instance.UpdateWeight();

                Instantiate(droppedItem, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);

                // Check to make sure the uint doesn't go to max value
                // because of a negative value being calculated
                if (currentWeight > 50) {
                    currentWeight = 0;
                }
            }
        }

        public void DamagePlayer(uint _damage) {
            if (currentWeight >= 1) {
                currentWeight -= _damage;
                Controllers.GameController.instance.amountDestroyed++;
                Controllers.HUDController.instance.UpdateWeight();
                RecalcWeight();
            }

            StartCoroutine(flash.FlashRoutine());
            Utils.ScreenShake.instance.ShakeScreen();
            AudioSource.PlayClipAtPoint(hurtSFX, transform.position, 2f);
        }

        public void RecalcWeight() {
            float weightRatio = currentWeight / 50f;
            weightRatio = Mathf.Clamp01(weightRatio);

            playerMovement.moveSpeed = playerMovement.maxMoveSpeed * (1f - 0.5f * weightRatio);
            playerMovement.jumpPower = 5f * (1f - 0.5f * weightRatio);
            playerMovement.wallJumpPower = new Vector2(5f, 5f * (1f - 0.5f * weightRatio));
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
            if (Utils.Exclaim.instance.exclaimActive) {
                AudioSource.PlayClipAtPoint(tinkSFX, transform.position, 2f);
            }
            pick.SetActive(true);
        }

        public void DeactivatePick() {
            Debug.Log("Pick deactivated");
            pick.SetActive(false);
        }
    }
}
