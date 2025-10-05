using UnityEngine;

namespace Utils {
    public class Exclaim : MonoBehaviour {

        private SpriteRenderer sprite;

        private void Awake() {
            sprite = GetComponent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Vein") || _collision.CompareTag("Chest")) {
                sprite.enabled = true;
            } 
        }

        void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Vein") || _collision.CompareTag("Chest")) {
                sprite.enabled = false;
            } 
        }
    }
}