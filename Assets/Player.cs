using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private Vector2 movementVector;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private TMP_Text text;
    private int score = 0;

    [SerializeField] private GameObject noteTextDialogue;
    [SerializeField] private GameObject hippieTextDialogue;

    private bool canPortal = false;
    
    private void Start() {
        text.SetText("Score:" + score);
    }

    private void FixedUpdate() {
        rb.velocity = movementVector * movementSpeed;
    }

    private void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movementVector = new Vector2(x, y);
        movementVector.Normalize();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Pineapple")) {
            score++;
            text.SetText("Score: " + score);

            // destroy the object
            Destroy(other.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Note")) {
            noteTextDialogue.SetActive(true);
            canPortal = true;
        }

        if (other.gameObject.CompareTag("Hippie")) {
            hippieTextDialogue.SetActive(true);
        }

        if (other.gameObject.CompareTag("Portal")) {
            if (!canPortal) return;
            transform.position = new Vector3(0, 0, 0);
        }

        if (other.gameObject.CompareTag("Kid")) {
            score = 0;
            text.SetText("Score: " + score);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Note")) {
            noteTextDialogue.SetActive(false);
        }
        
        
        if (other.gameObject.CompareTag("Hippie")) {
            hippieTextDialogue.SetActive(false);
        }
    }
}