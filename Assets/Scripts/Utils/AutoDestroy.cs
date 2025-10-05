using UnityEngine;

namespace Utils {
    public class AutoDestroy : MonoBehaviour {
        private float targetAlpha = 0;
        private float fadeSpeed = 1f;

        private SpriteRenderer sprite;

        private void Start() {
            sprite = GetComponent<SpriteRenderer>();
            Destroy(gameObject, 1.5f);
        }

        void Update() {
            Color c = sprite.color;
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            sprite.color = c;
        }
    }
}