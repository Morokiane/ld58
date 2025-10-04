using UnityEngine;
using UnityEngine.InputSystem;

namespace  Player {
    public class Player : MonoBehaviour {
        public static Player instance;
        private static readonly int mining = Animator.StringToHash("mining");

        [SerializeField] private GameObject pick;
        [SerializeField] private uint maxWeight = 50;

        private bool isMining;
        public uint currentWeight;

        private Animator anim;
        private Utils.Flash flash;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }

            anim = GetComponent<Animator>();
            flash = GetComponent<Utils.Flash>();
            
            pick.SetActive(false);
        }

        private void Start() {
            currentWeight = maxWeight;
            Debug.Log("Player weight " + currentWeight);
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

        public void DamagePlayer(uint _damage) {
            currentWeight -= _damage;
            StartCoroutine(flash.FlashRoutine());
            Utils.ScreenShake.instance.ShakeScreen();
            Controllers.HUDController.instance.UpdateWeight();
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
