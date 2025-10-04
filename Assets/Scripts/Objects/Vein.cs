using UnityEngine;

namespace Objects {
    public class Vein : MonoBehaviour {

        [SerializeField] private uint oreAmount;
        [SerializeField] private GameObject ore;

        private void Start() {
            oreAmount = (uint)Random.Range(1, 8);

            Debug.Log("Ore Amount: " + oreAmount);
        }

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Pick")) {
                if (oreAmount > 0) {
                    oreAmount--;
                    Instantiate(ore, transform.position, Quaternion.identity);
                }
            }

            if (oreAmount == 0) {
                Destroy(gameObject);
            }
        }
    }
}
