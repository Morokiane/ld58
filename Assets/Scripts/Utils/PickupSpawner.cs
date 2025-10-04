using UnityEngine;

namespace Utils {
    public class PickupSpawner : MonoBehaviour {
        [SerializeField] private GameObject itemPrefab;

        public void DropItems() {
            Instantiate(itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
