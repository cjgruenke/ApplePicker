using UnityEngine;

// Attached to the Branch prefab. Missing a branch is good, so it just cleans itself up.
public class Branch : MonoBehaviour {
    public static float bottomY = -20f;

    void Update() {
        if (transform.position.y < bottomY) {
            Destroy(this.gameObject);
        }
    }
}