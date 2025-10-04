using UnityEngine;
using UnityEngine.InputSystem;

namespace  Player {
    public class Player : Controllers.Singleton<Player> {
        private static readonly int mining = Animator.StringToHash("mining");

        [SerializeField] private GameObject pick;

        private bool isMining;

        private Animator anim;

        protected override void Awake() {
            base.Awake();

            anim = GetComponent<Animator>();
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

        public void ActivatePick() {
            pick.SetActive(true);
        }

        public void DeactivatePick() {
            pick.SetActive(false);
        }
    }
}
