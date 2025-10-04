using UnityEngine;

namespace Utils {
public class Destroy : MonoBehaviour {
        private LayerMask wallLayer;
        
        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player") || _collision.CompareTag("Wall")) {
                // Debug.Log("Did " + damage + " to player");
                Destroy(gameObject);
            }
        }
    }
}
