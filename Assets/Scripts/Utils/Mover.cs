using System.Collections;
using UnityEngine;

namespace Utils {
    public class Mover : MonoBehaviour {
        public enum MovementType : byte {
            Straight,
            Sine,
            Pursue,
            Stop
        }

        public MovementType movementType;
        [Header("Movement speeds")]
        public float moveSpeed = 5;
        public bool matchPlayerSpeed;
        [SerializeField] private bool reverseDirection;
        [Tooltip("Destroy the object instead of deactivating")]
        [SerializeField] private bool destroyObject;
        [SerializeField] private float destroyAt = -17;
        [Header("Sine settings")]
        [Tooltip("Height of movement")]
        [SerializeField] private float amplitude;
        [Tooltip("Speed of amplitude - Higher = faster")]
        [SerializeField] private float frequency;
        [SerializeField] private bool invert;
        [Header("Target Settings")]
        [HideInInspector] public bool facePlayer;
        [SerializeField] private bool stopRandom;
        [SerializeField] private bool stop;
        [SerializeField] private Vector2 moveTo;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        [Header("Bounds")]
        [Tooltip("Bounds that object can move within")]
        [SerializeField] private Vector2[] bounds;
        // Only going to use this for one time enemies like bosses

        private float sineCenterY;
        private Vector2 randomTarget; // Store the random target position
        private bool hasRandomTarget; // Flag to check if random target is set

        private void Start() {
            switch (movementType) {
                case MovementType.Pursue:
                    // pursuePlayer = true;
                    break;
                case MovementType.Sine:
                    sineCenterY = transform.position.y;
                    break;
                case MovementType.Straight:
                    break;
                case MovementType.Stop:
                    StartCoroutine(Stop());
                    break;
                default:
                    return;
            }
        }

        private void FixedUpdate() {
            if (!Player.Player.instance.isActiveAndEnabled)
                movementType = MovementType.Straight;
            
            var pos = SineMovement();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation

            if (movementType == MovementType.Pursue)
                PursuePlayer();
            else
                StraightMovement(pos);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void StraightMovement(Vector2 pos) {
            if (!stop && movementType != MovementType.Pursue) {
                // Regular movement logic
                if (!matchPlayerSpeed) {
                    if (reverseDirection)
                        pos.x += moveSpeed * Time.fixedDeltaTime;
                    else
                        pos.x -= moveSpeed * Time.fixedDeltaTime;
                } 

                if (pos.x < destroyAt && destroyObject) {
                    Destroy(gameObject);
                } else if (pos.x < -17 && !destroyObject) {
                    gameObject.SetActive(false);
                }

                transform.position = pos; // Update position as usual
            } else {
                if (!hasRandomTarget) {
                    randomTarget = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                    hasRandomTarget = true;
                }
                // Move towards a specific position when "stop" is enabled
                transform.position = Vector2.MoveTowards(transform.position, randomTarget, moveSpeed * Time.fixedDeltaTime);

                // Optionally, check if the enemy has reached the target position
                if (Vector2.Distance(transform.position, randomTarget) < 0.1f) {
                    // You can trigger further actions here once the enemy reaches the position
                    Debug.Log("Enemy reached target position.");
                    hasRandomTarget = false;
                    moveSpeed = 0;
                }
            }
        }

        private Vector2 SineMovement() {
            Vector2 pos = transform.position;

            if (movementType != (MovementType.Sine)) return pos;
            float sine = Mathf.Sin(pos.x * frequency) * amplitude;

            if (invert) {
                sine *= -1;
            }

            pos.y = sineCenterY + sine;
            return pos;
        }

        private void PursuePlayer() {
            Vector2 playerPos = Player.Player.instance.transform.position;
            Vector2 objectPos = transform.position;
            transform.position = Vector2.MoveTowards(objectPos, playerPos, moveSpeed * Time.fixedDeltaTime);

            if (facePlayer) {
                Vector2 direction = (playerPos - objectPos).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle += 180;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        private IEnumerator Stop() {
            Debug.Log("Stopping...");
            if (bounds == null || bounds.Length == 0) yield break;

                Vector2 min = bounds[0];
                Vector2 max = bounds[0];
                // Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
                // Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

                foreach (Vector2 point in bounds) {
                    if (point.x < min.x) min.x = point.x;
                    if (point.y < min.y) min.y = point.y;

                    if (point.x > max.x) max.x = point.x;
                    if (point.y > max.y) max.y = point.y;
                }

                if (Mathf.Approximately(min.x, max.x) || Mathf.Approximately(min.y, max.y)) yield break;

                var randomPosition = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));

                yield return transform.position = Vector2.MoveTowards(transform.position, randomPosition, moveSpeed * Time.fixedDeltaTime);
            }
    }
}
