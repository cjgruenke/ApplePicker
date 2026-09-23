using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Attached to Main Camera. Manages baskets, rounds, game over, and restart.
public class ApplePicker : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject basketPrefab;
    public int numBaskets = 4;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;
    public int pointsPerRound = 200;   // Points needed to complete each round
    public int numRounds = 4;          // Completing this round wins the game
    public TextMeshProUGUI roundText;
    public GameObject restartButton;

    [Header("Set Dynamically")]
    public List<GameObject> basketList;
    public bool gameOver = false;
    public int round = 1;              // Current round
    public int roundPoints = 0;        // Points earned so far in the current round

    void Start() {
        // Create the baskets, stacked from the bottom up
        basketList = new List<GameObject>();
        for (int i = 0; i < numBaskets; i++) {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);
            Vector3 pos = Vector3.zero;
            pos.y = basketBottomY + (basketSpacingY * i);
            tBasketGO.transform.position = pos;
            basketList.Add(tBasketGO);
        }

        round = 1;
        roundPoints = 0;
        restartButton.SetActive(false);
        UpdateRoundText();
    }

    // Called by Basket.cs every time an apple is caught
    public void AppleCaught(int points) {
        if (gameOver) return;

        roundPoints += points;

        if (roundPoints >= pointsPerRound) {
            if (round >= numRounds) {
                // Final round completed: the player wins
                GameOver(true);
            } else {
                // Move on to the next round
                round++;
                roundPoints = 0;
                UpdateRoundText();
            }
        }
    }

    // Called by Apple.cs when an apple falls off the bottom of the screen
    public void AppleDropped() {
        if (gameOver) return;

        // Clear everything currently falling
        DestroyFallingObjects();

        // Remove the top basket
        int basketIndex = basketList.Count - 1;
        GameObject tBasketGO = basketList[basketIndex];
        basketList.RemoveAt(basketIndex);
        Destroy(tBasketGO);

        // Losing every basket ends the game (not a win)
        if (basketList.Count == 0) {
            GameOver();
        }
    }

    void UpdateRoundText() {
        roundText.text = "Round " + round;
    }

    void DestroyFallingObjects() {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Apple")) {
            Destroy(go);
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Branch")) {
            Destroy(go);
        }
    }

    // playerWon is true only when all rounds are completed.
    // Calling GameOver() with no value (branch caught, all baskets lost) means playerWon = false.
    public void GameOver(bool playerWon = false) {
        if (gameOver) return;
        gameOver = true;

        DestroyFallingObjects();

        if (playerWon) {
            roundText.text = "Game Over YOU WIN!";
        } else {
            roundText.text = "Game Over";
        }

        restartButton.SetActive(true);

        // If this line errors on an older Unity version, use FindObjectOfType<AppleTree>() instead
        AppleTree tree = FindFirstObjectByType<AppleTree>();
        if (tree != null) tree.StopDropping();
    }

    // Hooked up to RestartButton's On Click ()
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}