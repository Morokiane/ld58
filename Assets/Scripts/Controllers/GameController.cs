using UnityEngine;

namespace Controllers {
    public class GameController : MonoBehaviour {
        public static GameController instance;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        public void ExitGame() {
            Application.Quit();
        }
    }    

}
