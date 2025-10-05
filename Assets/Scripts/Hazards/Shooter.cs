using UnityEngine;

namespace Hazards {
    public class Shooter : MonoBehaviour {
        [SerializeField] private GameObject fireball;
        [SerializeField] private float countdown;
        [SerializeField] private float countSpeed;
        [SerializeField] private AudioClip shootSFX;

        private float currentCount;
        [HideInInspector] public bool foundPlayer;

        private void Update() {
            if (currentCount <= 0 && foundPlayer) {
                GameObject fb = Instantiate(fireball, (new Vector2(transform.position.x, transform.position.y + 0.5f)), Quaternion.identity);
                AudioSource.PlayClipAtPoint(shootSFX, transform.position, 2f);
                
                if (transform.localScale.x < 0) {
                    fb.GetComponent<Utils.Mover>().reverseDirection = true;

                    Vector2 scale = fb.transform.localScale;
                    scale.x *= -1;
                    fb.transform.localScale = scale;
                }

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
                foundPlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                foundPlayer = false;
            }
        }
    }
}