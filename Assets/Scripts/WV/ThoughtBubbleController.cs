using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubbleController : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Bubble")]
    [Tooltip("The bubble should be a child of the monster")]
    public GameObject bubbleUI; // The whole bubble (with sprite + item icon)

    [Header("Item Image")]
    public Image wantedItemIcon; // For example, the sphere icon

    private void LateUpdate()
    {
        // Make the bubble always face the camera
        transform.LookAt(transform.position + cameraTransform.forward);
    }

    public void SetWantedItem(Image itemIcon)
    {
        wantedItemIcon.gameObject.SetActive(false); // Hide old
        wantedItemIcon = itemIcon;
        wantedItemIcon.gameObject.SetActive(true); // Show new
    }

    public void HideBubble()
    {
        bubbleUI.SetActive(false);
    }

    public void ShowBubble()
    {
        bubbleUI.SetActive(true);
    }
}
