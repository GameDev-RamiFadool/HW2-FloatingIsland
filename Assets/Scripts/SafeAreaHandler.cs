
using UnityEngine;
using DeviceScreen = UnityEngine.Device.Screen;

// Keeps UI elements inside the safe area of the screen.
[RequireComponent(typeof(RectTransform))]
public class SafeAreaHandler : MonoBehaviour
{
    // The UI rectangle that will fit inside the safe area.
    private RectTransform rectTransform;

    // Stores the previous screen settings to detect changes.
    private Rect previousSafeArea;
    private int previousWidth;
    private int previousHeight;

    // Called when the object is initialized.
    // Gets the RectTransform component.
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Called when the game starts.
    // Adjusts the UI to the current screen.
    private void Start()
    {
        UpdateSafeArea();
    }

    // Called every frame.
    // Checks if the screen size or safe area has changed.
    private void Update()
    {
        if (DeviceScreen.safeArea != previousSafeArea ||
            DeviceScreen.width != previousWidth ||
            DeviceScreen.height != previousHeight)
        {

            UpdateSafeArea();
        }
    }

    // Updates the UI boundaries to avoid notches and unsafe areas.
    private void UpdateSafeArea()
    {
        // Avoid calculating with invalid screen dimensions.
        if (DeviceScreen.width <= 0 || DeviceScreen.height <= 0)
        {
            return;
        }

        // Get the safe area of the current device.
        Rect safeArea = DeviceScreen.safeArea;

        // Calculate the bottom-left and top-right corners.
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // Convert screen pixels into normalized anchor values.
        anchorMin.x /= DeviceScreen.width;
        anchorMin.y /= DeviceScreen.height;
        anchorMax.x /= DeviceScreen.width;
        anchorMax.y /= DeviceScreen.height;

        // Fit the UI rectangle inside the safe area.
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        // Remember the screen settings for the next check.
        previousSafeArea = safeArea;
        previousWidth = DeviceScreen.width;
        previousHeight = DeviceScreen.height;
    }
}
