using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils {
    public class Exit : MonoBehaviour {
        void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                SceneManager.LoadScene("Scenes/EndGame");
            } 
        }
    }
}
