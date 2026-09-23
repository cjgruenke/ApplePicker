using UnityEngine;
using TMPro;

// Attached to the HighScore text. Saves the high score between play sessions with PlayerPrefs.
public class HighScore : MonoBehaviour {
    public static int score = 1000;

    private TextMeshProUGUI gt;

    void Awake() {
        gt = GetComponent<TextMeshProUGUI>();

        // Load a saved high score if one exists
        if (PlayerPrefs.HasKey("HighScore")) {
            score = PlayerPrefs.GetInt("HighScore");
        }
        PlayerPrefs.SetInt("HighScore", score);
    }

    void Update() {
        gt.text = "High Score: " + score;

        // Save if the high score was beaten
        if (score > PlayerPrefs.GetInt("HighScore")) {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}