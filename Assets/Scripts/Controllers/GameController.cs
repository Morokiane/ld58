using UnityEngine;

namespace Controllers {
    public class GameController : MonoBehaviour {
        public static GameController instance;

        public uint amountCollected;
        public uint amountTrashed;
        public uint amountDestroyed;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
    }    

}
