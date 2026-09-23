using UnityEngine;
using TMPro;

// Attached to the Basket prefab. Follows the mouse and catches apples and branches.
public class Basket : MonoBehaviour {
    [Header("Set Dynamically")]
    public TextMeshProUGUI scoreGT;

    private ApplePicker apScript;

    void Start() {
        // Find the score text by name (must be exactly "ScoreCounter")
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
        scoreGT.text = "0";

        apScript = Camera.main.GetComponent<ApplePicker>();
    }

    void Update() {
        // Freeze baskets once the game is over
        if (apScript != null && apScript.gameOver) return;

        // Convert the mouse position from screen space to world space
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Move only along x to follow the mouse
        Vector3 pos = transform.position;
        pos.x = mousePos3D.x;
        transform.position = pos;
    }

    void OnCollisionEnter(Collision coll) {
        if (apScript.gameOver) return;

        GameObject collidedWith = coll.gameObject;

        if (collidedWith.CompareTag("Apple")) {
            // Stop the same apple from being counted twice if it touches two baskets
            collidedWith.tag = "Untagged";
            Destroy(collidedWith);

            int score = int.Parse(scoreGT.text);
            score += 100;
            scoreGT.text = score.ToString();

            if (score > HighScore.score) {
                HighScore.score = score;
            }

            // Tell ApplePicker so it can track round progress
            apScript.AppleCaught(100);
        } else if (collidedWith.CompareTag("Branch")) {
            Destroy(collidedWith);
            apScript.GameOver();
        }
    }
}