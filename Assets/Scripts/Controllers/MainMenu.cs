using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers {
    public class MainMenu : MonoBehaviour {

        public void StartGame() {
            SceneManager.LoadScene("Scenes/Master");
        }
        
        public void ExitGame() {
            Application.Quit();
        }
    }
}