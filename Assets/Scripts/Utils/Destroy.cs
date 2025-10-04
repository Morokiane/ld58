using UnityEngine;

namespace Utils {
public class Destroy : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player") || _collision.CompareTag("Wall") || _collision.CompareTag("Obstacle")) {
                // Debug.Log("Did " + damage + " to player");
                Destroy(gameObject);
            }
        }
    }
}
