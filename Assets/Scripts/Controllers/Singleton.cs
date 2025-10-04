using UnityEngine;

namespace Controllers {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static T _instance;
        public static T instance { get { return instance; } }

        protected virtual void Awake() {
            if (_instance != null && this.gameObject != null) {
                Destroy(this.gameObject);
            } else {
                _instance = (T)this;
            }

            DontDestroyOnLoad(transform.root.gameObject);
        }
    }
}
