using UnityEngine;

namespace Objects {
    public class Chest : MonoBehaviour {
        [SerializeField] private GameObject coin;
        [SerializeField] private Sprite sprite;

        private bool isOpen;
        private uint coinAmount;

        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider2D;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            coinAmount = (uint)Random.Range(5, 15);
        }

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Pick") && !isOpen) {
                while (coinAmount > 0) {
                    isOpen = true;
                    coinAmount--;
                    Instantiate(coin, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                }
                spriteRenderer.sprite = sprite;
                circleCollider2D.enabled = false;
            }
        }
    }
}
