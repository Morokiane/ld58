using UnityEngine;

namespace Hazards {
    public class Spikes : MonoBehaviour {
        [SerializeField] private uint damage;
        [SerializeField] private float countdown;
        [SerializeField] private float countSpeed;

        private float currentCount;
        private bool foundPlayer;

        private void Update() {
            if (currentCount <= 0 && foundPlayer) {
                Damage(damage);
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
                Damage(damage);
                foundPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                foundPlayer = false;
            }
        }

        private void Damage(uint _damage) {
            Player.Player.instance.DamagePlayer(_damage);
            Debug.Log("Damaged player for " + _damage);
        }
    }
}
