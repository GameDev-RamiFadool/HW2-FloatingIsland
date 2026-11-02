
using UnityEngine;

// Makes a cloud move slowly back and forth in the sky.
public class CloudMovement : MonoBehaviour
{
    // How far the cloud moves from its starting position.
    [SerializeField] private float moveDistance = 0.3f;

    // Controls how fast the cloud moves.
    [SerializeField] private float moveSpeed = 0.5f;

    // Gives different clouds different movement timing.
    [SerializeField] private float phaseOffset = 0f;

    // Stores the cloud's original position.
    private Vector3 startPosition;

    // Called when the game starts.
    // Saves the cloud's starting position.
    private void Start()
    {
        startPosition = transform.localPosition;
    }

    // Called every frame.
    // Moves the cloud smoothly from side to side.
    private void Update()
    {
        // Calculate smooth horizontal movement.
        float offset = Mathf.Sin(Time.time * moveSpeed + phaseOffset)
            * moveDistance;

        // Move the cloud without changing its original position.
        transform.localPosition =
            startPosition + Vector3.right * offset;
    }
}
