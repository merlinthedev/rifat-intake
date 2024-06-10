using UnityEngine;

public class Kid : MonoBehaviour {
    [SerializeField] private Sprite sprite;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
    }
}