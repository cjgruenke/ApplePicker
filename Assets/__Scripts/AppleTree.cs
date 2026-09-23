using UnityEngine;

// Attached to the AppleTree parent object. Moves back and forth and drops apples and branches.
public class AppleTree : MonoBehaviour {
    [Header("Set in Inspector")]
    public GameObject applePrefab;
    public GameObject branchPrefab;
    public float speed = 10f;                       // Movement speed in units per second
    public float leftAndRightEdge = 20f;            // Distance where the tree turns around
    public float chanceToChangeDirections = 0.02f;  // Chance per physics step to reverse
    public float secondsBetweenAppleDrops = 1f;
    [Range(0f, 1f)]
    public float branchChance = 0.08f;              // Fraction of drops that are branches

    private bool dropping = true;

    void Start() {
        // Start dropping after 2 seconds
        Invoke("DropApple", 2f);
    }

    void DropApple() {
        if (!dropping) return;

        // Usually drop an apple; occasionally drop a branch
        GameObject prefab = (Random.value < branchChance) ? branchPrefab : applePrefab;
        GameObject drop = Instantiate<GameObject>(prefab);
        drop.transform.position = transform.position;

        // Schedule the next drop
        Invoke("DropApple", secondsBetweenAppleDrops);
    }

    // Called by ApplePicker.GameOver()
    public void StopDropping() {
        dropping = false;
        CancelInvoke("DropApple");
    }

    void Update() {
        // Basic movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Turn around at the edges
        if (pos.x < -leftAndRightEdge) {
            speed = Mathf.Abs(speed);        // Move right
        } else if (pos.x > leftAndRightEdge) {
            speed = -Mathf.Abs(speed);       // Move left
        }
    }

    void FixedUpdate() {
        // Random direction changes. FixedUpdate runs at a fixed rate,
        // so this doesn't depend on frame rate.
        if (Random.value < chanceToChangeDirections) {
            speed *= -1;
        }
    }
}