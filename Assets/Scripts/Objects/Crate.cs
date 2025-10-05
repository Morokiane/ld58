using UnityEngine;

namespace Objects {
    public class Crate : MonoBehaviour {
        [SerializeField] private uint health = 10;
        [SerializeField] private AudioClip tinkSFX;

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Pick") && health > 0) {
                health--;
                AudioSource.PlayClipAtPoint(tinkSFX, transform.position, 2f);
                if (health <= 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}