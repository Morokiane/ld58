using UnityEngine;

namespace Hazards {
    public class Shooter : MonoBehaviour {
        [SerializeField] private GameObject fireball;
        [SerializeField] private float countdown;
        [SerializeField] private float countSpeed;

        private float currentCount;
        [HideInInspector] public bool foundPlayer;

        private void Update() {
            if (currentCount <= 0 && foundPlayer) {
                Instantiate(fireball, (new Vector2(transform.position.x, transform.position.y + 0.5f)), Quaternion.identity);
                currentCount = countdown;
            }
        }

        private void FixedUpdate() {
            if (foundPlayer) {
                currentCount = currentCount - (countSpeed * Time.fixedDeltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                foundPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                foundPlayer = false;
            }
        }
    }
}