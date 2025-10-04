using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Utils {
    public class ScreenShake : MonoBehaviour {
        public static ScreenShake instance;
        private CinemachineImpulseSource source;

        private void Awake() {
            instance = this;
            
            source = GetComponent<CinemachineImpulseSource>();
        }

        public void ShakeScreen() {
            source.GenerateImpulse();
        }
    }
}