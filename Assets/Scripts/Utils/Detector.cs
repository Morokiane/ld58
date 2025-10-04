using UnityEngine;

namespace Utils {
    public class Detector : MonoBehaviour {
        [SerializeField] private Hazards.Shooter shooter;

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                shooter.foundPlayer = true;
            }
        }

        void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                shooter.foundPlayer = false;
            }
        }
    }
}
