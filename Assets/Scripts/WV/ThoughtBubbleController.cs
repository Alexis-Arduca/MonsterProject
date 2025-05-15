using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubbleController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("The camera that the bubble will face.")]
    public Transform cameraTransform;

    [Header("Item Image")]
    [Tooltip("The icon that represents the item the monster wants.")]
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
        gameObject.SetActive(false);
    }

    public void ShowBubble()
    {
        gameObject.SetActive(true);
    }
}
