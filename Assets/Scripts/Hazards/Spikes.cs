using UnityEngine;

namespace Hazards {
    public class Spikes : MonoBehaviour {
        [SerializeField] private float countdown;
        [SerializeField] private float countSpeed;

        private float currentCount;
        private bool foundPlayer;

        private void Update() {
            if (currentCount <= 0 && foundPlayer) {
                Debug.Log("Damage player");
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
