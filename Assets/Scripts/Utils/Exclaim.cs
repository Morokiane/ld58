using UnityEngine;

namespace Utils {
    public class Exclaim : MonoBehaviour {

        private SpriteRenderer sprite;

        private void Awake() {
            sprite = GetComponent<SpriteRenderer>();
        }

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Vein")) {
                sprite.enabled = true;
            } 
        }

        void OnTriggerExit2D(Collider2D collision) {
            if (collision.CompareTag("Vein")) {
                sprite.enabled = false;
            } 
        }
    }
}