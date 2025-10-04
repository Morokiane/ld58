using UnityEngine;

namespace Objects {
    public class Vein : MonoBehaviour {

        [SerializeField] private uint oreAmount;
        [SerializeField] private GameObject ore;

        private void Start() {
            oreAmount = (uint)Random.Range(2, 6);
        }

        void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Pick")) {
                if (oreAmount > 0) {
                    oreAmount--;
                    // I have to move this up or the ore will end up in the tiles
                    Instantiate(ore, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                }
            }

            if (oreAmount == 0) {
                Destroy(gameObject);
            }
        }
    }
}
