using UnityEngine;

namespace Utils {
    public class Exclaim : MonoBehaviour {
        public static Exclaim instance;

        public bool exclaimActive { set; get; }
        private SpriteRenderer sprite;

        private void Awake() {
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            instance = this;
        }

        void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Vein") || _collision.CompareTag("Chest")) {
                exclaimActive = true;
                sprite.enabled = true;
            } 
        }

        void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Vein") || _collision.CompareTag("Chest")) {
                exclaimActive = false;
                sprite.enabled = false;
            } 
        }
    }
}