
using UnityEngine;

// Adjusts the camera size when the screen resolution or orientation changes.
[RequireComponent(typeof(Camera))]
public class CameraAspectController : MonoBehaviour
{
    // The screen resolution used as our reference.
    [SerializeField]
    private Vector2 referenceResolution =
        new Vector2(1920f, 1080f);

    // The camera size at the reference resolution.
    [SerializeField] private float referenceOrthographicSize = 5f;

    // Controls how much the camera zooms in on portrait screens.
    // A smaller value makes the island appear larger.
    [SerializeField] private float portraitZoomFactor = 0.8f;

    // Stores the camera attached to this object.
    private Camera cameraComponent;

    // Stores the previous screen dimensions.
    private int previousWidth;
    private int previousHeight;

    // Called once when the object is initialized.
    // Gets the Camera component.
    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    // Called when the game starts.
    // Sets the camera size for the current screen.
    private void Start()
    {
        UpdateCameraSize();
    }

    // Called every frame.
    // Checks if the screen size or orientation has changed.
    private void Update()
    {
        if (Screen.width != previousWidth ||
            Screen.height != previousHeight)
        {
            UpdateCameraSize();
        }
    }

    // Calculates the camera size using the screen aspect ratio.
    // Keeps the same horizontal world area visible on different screens.
    private void UpdateCameraSize()
    {
        // Avoid calculating with invalid screen dimensions.
        if (Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        // Calculate the reference and current aspect ratios.
        float referenceAspect =
            referenceResolution.x / referenceResolution.y;

        float currentAspect = (float)Screen.width / Screen.height;


        // Calculate the camera size for the current screen.
        float newSize = referenceOrthographicSize * referenceAspect / currentAspect;

        // Zoom in a little when the screen is in portrait mode.
        if (Screen.height > Screen.width)
        {
            newSize *= portraitZoomFactor;
        }

        // Apply the new camera size.
        cameraComponent.orthographicSize = newSize;


        // Remember the current screen dimensions.
        previousWidth = Screen.width;
        previousHeight = Screen.height;
    }
}
