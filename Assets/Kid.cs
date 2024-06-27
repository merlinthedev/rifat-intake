using UnityEngine;

public class Kid : MonoBehaviour {
    [SerializeField] private Sprite sprite; // reference to the happy sprite, the sad one is the default one, we dont need a reference to that.

    private void OnTriggerEnter2D(Collider2D other) {
        // if the player hits the kid, make sure the happy sprite is activated.
        // technically this will always change the sprite regardless of the player having score
        // this will also still change sprite even if it has already been changed before, except you dont see that :)
        if (other.gameObject.CompareTag("Player")) {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
    }
}