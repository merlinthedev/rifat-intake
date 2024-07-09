using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    private Vector2 movementVector;
    [SerializeField] private float movementSpeed = 5f; // serialized so we can change it in the inspector
    [SerializeField] private Rigidbody2D rb; // reference to the rigidbody so we can apply velocity to it, make sure to drag it in the scene!

    [SerializeField] private TMP_Text text; // score ui text object. make sure to drag it in the scene!
    private int score = 0;

    [SerializeField] private GameObject noteTextDialogue; // reference to the note dialogue, make sure to drag it in the scene!
    [SerializeField] private GameObject hippieTextDialogue; // reference to the hippie dialogue, make sure to drag it in the scene!

    private bool canPortal = false; // only if this is true, the player can use the portal. this will be set to true after hitting the note.
    private Animator animator; // reference to the animator so we can change the sprite of the player
    private bool isMoving = false; // bool to check if the player is moving or not
    private SpriteRenderer spriteRenderer; // reference to the sprite renderer so we can change the sprite of the player

    [SerializeField] private GameObject portal; // reference to the portal so we can set it to active or inactive
    [SerializeField] private Transform teleportDestination; // reference to the teleport destination so we can teleport the player to the center of the world
    
    private void Start() {
        text.SetText("Score:" + score); // make sure the default text at the start of the game says "Score: 0"
        animator = GetComponent<Animator>(); // get the animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // get the sprite renderer component
    }

    private void FixedUpdate() {
        // set the velocity of the rigidbody in fixed update so it is framerate independent
        rb.velocity = movementVector * movementSpeed; 
    }

    private void Update() {
        // get the input from the keyboard on both axis
        float x = Input.GetAxisRaw("Horizontal"); 
        float y = Input.GetAxisRaw("Vertical");

        // set the movement vector to the input so we can later apply this to the rigidbody
        // we do this because this way we make sure that we dont lose any inputs but the application of the movement is framerate independent
        movementVector = new Vector2(x, y);
        movementVector.Normalize();
        
        if(movementVector != Vector2.zero) {
            animator.SetBool("isMoving", true);
            if(!isMoving) animator.SetTrigger("Move");
            isMoving = true;
        } else {
            animator.SetBool("isMoving", false);
            isMoving = false;
        }
        
        // if we are moving to the right, we flip the sprite so it looks like the player is facing the right direction
        if (movementVector.x > 0) {
            spriteRenderer.flipX = true;
        }
        
        // if we are moving to the left, we flip the sprite so it looks like the player is facing the right direction
        else if (movementVector.x < 0) {
            spriteRenderer.flipX = false;
        }
        
    }

    // collision enter instead of trigger enter because the collider of the player is a solid collider and not a trigger
    private void OnCollisionEnter2D(Collision2D other) {
        // if we hit a pineapple, we add to the score and the ui. we also destroy the pineapple
        if (other.gameObject.CompareTag("Pineapple")) {
            score++;
            text.SetText("Pineapple: " + score);

            // destroy the object
            Destroy(other.gameObject);
        }
        
    }

    // trigger enter because the gameobjects we interact with here have a trigger collider
    private void OnTriggerEnter2D(Collider2D other) {
        // if we hit a note, activate the ui component and make sure we can portal afterwards
        if (other.gameObject.CompareTag("Note")) {
            noteTextDialogue.SetActive(true);
            canPortal = true;
            portal.gameObject.SetActive(true);
        }

        // if we hit the hippie, activate the ui component
        if (other.gameObject.CompareTag("Hippie")) {
            hippieTextDialogue.SetActive(true);
        }

        // if we hit the portal, first check if we can portal at all, if not we dont do anything afterwards, hence the return statement
        // if we can portal we tp the player to the center of the world, that is where our "earth world" is in the scene 
        if (other.gameObject.CompareTag("Portal")) {
            if (!canPortal) return;
            transform.position = teleportDestination.position;
        }
        
        // if we hit the kid, on the player side we only have to reset the score, the kid will take care of changing its sprite
        if (other.gameObject.CompareTag("Kid")) {
            score = 0;
            text.SetText("Pineapple: " + score);
        }
    }

    // if we exit any triggers that had dialogue activated because of them, make sure they deactivate
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Note")) {
            noteTextDialogue.SetActive(false); // deactivate
        }
        
        
        if (other.gameObject.CompareTag("Hippie")) {
            hippieTextDialogue.SetActive(false); // deactivate
        }
    }
}