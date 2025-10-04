using UnityEngine;

namespace Utils {
    public class Ore : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                Debug.Log("found player");
                Destroy(gameObject);
            }
        }
    }
}
