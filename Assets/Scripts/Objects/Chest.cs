using System.Collections;
using UnityEngine;

namespace Objects {
    public class Chest : MonoBehaviour {
        [SerializeField] private uint minCoins = 5;
        [SerializeField] private uint maxCoins = 15;
        [Header("")]
        [SerializeField] private GameObject coin;
        [SerializeField] private Sprite sprite;
        [SerializeField] private AudioClip coinsSFX;

        private bool isOpen;
        private uint coinAmount;

        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider2D;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider2D = GetComponent<CircleCollider2D>();

            coinAmount = (uint)Random.Range(minCoins, maxCoins);
        }

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Pick") && !isOpen) {
                while (coinAmount > 0) {
                    isOpen = true;
                    coinAmount--;
                    Instantiate(coin, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    AudioSource.PlayClipAtPoint(coinsSFX, transform.position, 2f);
                }
                spriteRenderer.sprite = sprite;
                circleCollider2D.enabled = false;
            }
        }

        private IEnumerator Jiggle() {
            Vector3 originalScale = transform.localScale;

            transform.localScale = originalScale * 0.9f;
            yield return new WaitForSeconds(0.05f);

            transform.localScale = originalScale * 1.1f;
            yield return new WaitForSeconds(0.05f);

            transform.localScale = originalScale;
        }
    }
}
