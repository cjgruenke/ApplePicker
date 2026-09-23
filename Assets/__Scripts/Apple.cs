using UnityEngine;

// Attached to the Apple prefab. Reports a miss when the apple falls off screen.
public class Apple : MonoBehaviour {
    public static float bottomY = -20f;

    void Update() {
        if (transform.position.y < bottomY) {
            Destroy(this.gameObject);

            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            apScript.AppleDropped();
        }
    }
}