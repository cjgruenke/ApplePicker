using UnityEngine;
using UnityEngine.SceneManagement;

// Attached to StartMenuManager in StartScene. Loads the game when Start is clicked.
public class StartMenu : MonoBehaviour {
    // Must exactly match the game scene's file name
    public string gameSceneName = "_Scene_0";

    // Hooked up to StartButton's On Click ()
    public void StartGame() {
        SceneManager.LoadScene(gameSceneName);
    }
}