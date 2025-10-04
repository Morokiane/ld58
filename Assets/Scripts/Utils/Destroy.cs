using UnityEngine;

namespace Utils {
public class Destroy : MonoBehaviour {
        [SerializeField] private uint damage;
        
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                // Debug.Log("Did " + damage + " to player");
                Destroy(gameObject);
            }
        }
    }
}
