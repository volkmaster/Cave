using UnityEngine;
using UnityEngine.UI;

namespace WorldGenerator {
    public class FPS : MonoBehaviour {
        private Text fpsText;
        private float deltaTime = 0.0f;

        // Start is called before the first frame update
        private void Start() {
            fpsText = gameObject.GetComponent<Text>();
        }

        // Update is called once per frame
        void Update() {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI() {
            float fps = 1.0f / deltaTime;
            fpsText.text = string.Format("{0:0.} fps", fps);
        }
    }
}