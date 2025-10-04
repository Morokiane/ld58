using UnityEngine;

namespace Utils {
public class DamagePlayer : MonoBehaviour {
        [SerializeField] private uint damageAmount;

        private void OnTriggerEnter2D(Collider2D _collision) {
            if (_collision.CompareTag("Player")) {
                Damage(damageAmount);
            }
        }
        
        private void Damage(uint _damage) {
            Player.Player.instance.DamagePlayer(_damage);
            Debug.Log("Damaged player for " + _damage);
        }
    }
}
