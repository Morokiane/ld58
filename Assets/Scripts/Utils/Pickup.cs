using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils {
    public class Pickup : MonoBehaviour {
        [SerializeField] private float pickupDistance = 3f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float accelRate = 0.5f;
        [SerializeField] private float initialForceX = 2f;
        [SerializeField] private float initialForceY = 6f;
        [SerializeField] private float collectibleDelay = 0.25f;
        [SerializeField] private bool magnetic = true;

        private Vector2 moveDir;
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;
        private bool isCollectible;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            circleCollider.enabled = false;
            isCollectible = false;

            float randomX = Random.Range(-initialForceX, initialForceX);
            float randomY = Random.Range(initialForceY * 0.8f, initialForceY * 1.2f);
            rb.linearVelocity = new Vector2(randomX, randomY);

            StartCoroutine(EnableCollection());
        }

        private IEnumerator EnableCollection() {
            yield return new WaitForSeconds(collectibleDelay);
            circleCollider.enabled = true;
            isCollectible = true;
        }

        private void FixedUpdate() {
            if (!magnetic || !isCollectible) return;

            Vector2 playerPos = Player.Player.instance.transform.position;

            if (Vector2.Distance(transform.position, playerPos) < pickupDistance) {
                moveDir = (playerPos - (Vector2)transform.position).normalized;
                moveSpeed += accelRate;

                rb.linearVelocity = moveDir * moveSpeed * Time.fixedDeltaTime;
            }
        }
    }

}
