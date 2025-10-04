using UnityEngine;

namespace Utils {
    public class Ore : MonoBehaviour {

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player") && Player.Player.instance.currentWeight < Player.Player.instance.maxWeight) {
                Player.Player.instance.currentWeight++;
                Controllers.HUDController.instance.UpdateWeight();
                Player.Player.instance.RecalcWeight();
                Destroy(gameObject);
            }
        }
    }
}
