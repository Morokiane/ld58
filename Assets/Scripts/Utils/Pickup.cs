using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils {
    public class Pickup : MonoBehaviour {
        public Item item;

        public enum Item : byte {
            Coin,
            Heart,
            Stamina
        }
        [Header("")]
        [SerializeField] private float pickupDistance;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float accelRate = 0.5f;
        [SerializeField] private float heightY = 2f;
        [SerializeField] private float popDuration = 0.25f;
        [SerializeField] private AnimationCurve animCurve;
        [Tooltip("Attracts to the player")]
        [SerializeField] private bool magnetic = true;
        
        private Vector2 moveDir;
        private Rigidbody2D rb;
        private CircleCollider2D circleCollider;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Start() {
            StartCoroutine(SpawnAnim());
            circleCollider.enabled = false;

            UpdateItem();
        }

        private void FixedUpdate() {
            if (!magnetic) return;

            Vector2 playerPos = Player.Player.instance.transform.position;

            if (Vector2.Distance(transform.position, playerPos) < pickupDistance) {
                moveDir = (playerPos - (Vector2)transform.position).normalized;
                moveSpeed += accelRate;
            } else {
                moveDir = Vector2.zero;
                moveSpeed = 0f;
            }

            rb.linearVelocity = moveDir * (moveSpeed * Time.fixedDeltaTime);
        }

        private void UpdateItem() {
            switch (item) {
                case Item.Coin:
                    magnetic = true;
                    break;
                case Item.Heart:
                    magnetic = false;
                    break;
                case Item.Stamina:
                    magnetic = false;
                    break;
            }
            
        }

        private IEnumerator SpawnAnim() {
            // This is for the arc animation after the pick up is spawned
            Vector2 startPoint = transform.position;
            float randomX = transform.position.x + Random.Range(-2f, 2f);
            float randomY = transform.position.y + Random.Range(-1f, 1f);

            Vector2 endPoint = new Vector2(randomX, randomY);
            float timePassed = 0f;

            while (timePassed < popDuration) {
                timePassed += Time.deltaTime;
                float linearT = timePassed / popDuration;
                float heightT = animCurve.Evaluate(linearT);
                float height = Mathf.Lerp(0f, heightY, heightT);

                // transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height);
                rb.MovePosition(Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0f, height));
                yield return null;
            }

            yield return new WaitForSeconds(1);
            circleCollider.enabled = true;
        }
    }
}
