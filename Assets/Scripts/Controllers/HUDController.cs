using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Controllers {
    public class HUDController : MonoBehaviour {
        public static HUDController instance;

        [SerializeField] private Slider fillBar;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        
        private void Start() {
            StartCoroutine(DelayUpdate());
        }

        public void UpdateWeight() {
            fillBar.value = Player.Player.instance.currentWeight;
            Debug.Log("HUD " + Player.Player.instance.currentWeight);
        }

        private IEnumerator DelayUpdate() {
            yield return new WaitForSeconds(0.5f);
            UpdateWeight();
        }
    }
}