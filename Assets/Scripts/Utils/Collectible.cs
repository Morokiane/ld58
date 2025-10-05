using UnityEngine;

namespace Utils {
    public class Ore : MonoBehaviour {
        [SerializeField] private uint weight;
        [SerializeField] private AudioClip collectSFX;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player") && Player.Player.instance.currentWeight < Player.Player.instance.maxWeight) {
                Player.Player.instance.currentWeight += weight;
                Controllers.HUDController.instance.UpdateWeight();
                Player.Player.instance.RecalcWeight();
                AudioSource.PlayClipAtPoint(collectSFX, transform.position, 2f);
                Controllers.GameController.instance.amountCollected++;
                Destroy(gameObject);
            }
        }
    }
}
