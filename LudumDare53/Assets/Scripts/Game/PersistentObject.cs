using System;
using UnityEngine;

namespace Game {
    public class PersistentObject : MonoBehaviour {

        public static PersistentObject Instance;

        private void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}