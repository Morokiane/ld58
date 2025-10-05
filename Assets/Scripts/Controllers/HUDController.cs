using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Controllers {
    public class HUDController : MonoBehaviour {
        public static HUDController instance;

        [SerializeField] private Slider fillBar;
        [SerializeField] private GameObject menu;

        private bool menuOpen;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        
        private void Start() {
            menu.SetActive(false);
            StartCoroutine(DelayUpdate());
        }

        public void UpdateWeight() {
            fillBar.value = Player.Player.instance.currentWeight;
        }

        private IEnumerator DelayUpdate() {
            yield return new WaitForSeconds(0.5f);
            UpdateWeight();
        }

        public void OpenMenu() {
            if (!menuOpen) {
                Player.PlayerMovement.instance.canMove = false;
                menuOpen = true;
                menu.SetActive(true);
            } else {
                Player.PlayerMovement.instance.canMove = true;
                menuOpen = false;
                menu.SetActive(false);
            }
        }

        public void Continue() {
            OpenMenu();
        }

        public void Leave() {
            SceneManager.LoadScene("Scenes/Title");
        }
    }
}