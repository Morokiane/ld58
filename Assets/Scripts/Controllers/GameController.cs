using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Controllers {
    public class GameController : MonoBehaviour {
        public static GameController instance;

        [SerializeField] private Renderer2DData rendererData;

        public uint amountCollected;
        public uint amountTrashed;
        public uint amountDestroyed;
        private ScriptableRendererFeature crtFilter;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }

            foreach (var feature in rendererData.rendererFeatures) {
                if (feature.name == "CRT Filter") {
                    crtFilter = feature;
                    break;
                }
            }
        }

        public void ToggleCRT(bool enabled) {
            if (crtFilter != null) {
                crtFilter.SetActive(enabled);
                rendererData.SetDirty();
            }
        }
    }    

}
