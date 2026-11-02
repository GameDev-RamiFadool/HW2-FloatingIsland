
using UnityEngine;
using UnityEngine.InputSystem;

// Controls the player's movement using WASD or arrow keys.
public class PlayerMovement : MonoBehaviour
{
    // Movement speed. Can be changed in the Inspector.
    [SerializeField] private float moveSpeed = 2f;

    // The area where the player is allowed to walk.
    [SerializeField] private PolygonCollider2D walkableArea;

    // The position of the player's feet relative to the sprite center.
    [SerializeField] private Vector2 feetOffset = new Vector2(0f, -0.35f);

    // Stores the movement input from the keyboard.
    private InputAction moveAction;

    // The component that plays the player's animations.
    private Animator animator;

    // Stores the last direction the player was facing.
    private string facingDirection = "Down";

    // Stores the current animation so it does not restart every frame.
    private string currentAnimation = "";


    // Called once when the object is initialized.
    // Creates the movement controls for WASD and arrow keys.
    private void Awake()
    {
        // Get the player's Animator component.
        animator = GetComponent<Animator>();

        moveAction = new InputAction("Move", InputActionType.Value);

        // Add WASD movement controls.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Add arrow keys.
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

    // Called when the script becomes active.
    // Enables the movement input.
    private void OnEnable()
    {
        moveAction.Enable();
    }

    // Called when the script becomes inactive.
    // Disables the movement input.
    private void OnDisable()
    {
        moveAction.Disable();
    }

    // Called when the game starts.
    // Checks if the walking area is connected.
    private void Start()
    {
        if (walkableArea == null)
        {
            Debug.LogError("Walkable Area is not assigned!", this);
        }
    }

    // Called every frame.
    // Moves the player based on the pressed keys.
    private void Update()
    {
        // Read the movement direction from the keyboard.
        Vector2 direction = moveAction.ReadValue<Vector2>();

        // Normalize the direction to avoid faster diagonal movement.
        direction = direction.normalized;

        // Remember the position before moving.
        Vector2 previousPosition = transform.position;

        // Move the player at the same speed regardless of FPS.
        Vector2 movement = direction * moveSpeed * Time.deltaTime;

        // Check each direction separately so the player can slide along edges.
        if (walkableArea != null)
        {
            TryMove(new Vector2(movement.x, 0f));
            TryMove(new Vector2(0f, movement.y));
        }

        // Check if the player actually moved.
        bool isMoving =
            ((Vector2)transform.position - previousPosition).sqrMagnitude > 0f;

        // Update the direction the player is looking at.
        UpdateFacingDirection(direction, isMoving);
    }


    // Tries to move the player to a new position.
    // Only allows movement if the player's feet stay inside the island.
    private void TryMove(Vector2 movement)
    {
        // Calculate the player's next position.
        Vector3 nextPosition = transform.position + (Vector3)movement;

        // Calculate where the player's feet will be.
        Vector2 nextFeetPosition = (Vector2)nextPosition + feetOffset;

        // Check if the feet are inside the walking area.
        if (walkableArea.OverlapPoint(nextFeetPosition))
        {
            // Move the player only if the position is valid.
            transform.position = nextPosition;
        }
    }


    // Updates the player's sprite based on the movement direction.
    // Keeps the last direction when the player stops moving.
    private void UpdateFacingDirection(Vector2 direction, bool isMoving)
    {
        // Only update the facing direction when a movement key is pressed.
        if (direction != Vector2.zero)
        {

            // Check if the player is moving mainly horizontally.
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    facingDirection = "Right";
                }
                else
                {
                    facingDirection = "Left";
                }
            }
            else
            {
                // Change the sprite for vertical movement.
                if (direction.y > 0)
                {
                    facingDirection = "Up";
                }
                else
                {
                    facingDirection = "Down";
                }
            }
        }

        // Play walking animation when moving, otherwise play idle.
        string animationName =
            (isMoving ? "Walk_" : "Idle_") + facingDirection;

        // Only switch animations when the animation needs to change.
        if (currentAnimation != animationName)
        {
            animator.Play(animationName);
            currentAnimation = animationName;
        }
    }

    // Called when the object is destroyed.
    // Releases the input action resources.
    private void OnDestroy()
    {
        moveAction.Dispose();
    }
}
