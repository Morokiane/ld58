using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Objects {
    public class Vein : MonoBehaviour {

        [SerializeField] private uint minAmount = 2;
        [SerializeField] private uint maxAmount = 6;
        [SerializeField] private GameObject ore;

        private uint oreAmount;

        private void Start() {
            oreAmount = (uint)Random.Range(minAmount, maxAmount);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Pick")) {
                if (oreAmount > 0) {
                    oreAmount--;
                    Instantiate(ore, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                }

                StartCoroutine(Jiggle());
            }

            if (oreAmount == 0) {
                Destroy(gameObject);
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
