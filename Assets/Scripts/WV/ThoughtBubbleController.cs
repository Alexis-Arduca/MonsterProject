using TMPro;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// Attached to the Thought Bubble Prefab.
// This script is responsible for controlling the thought bubble that appears above the monster.
// It handles the visibility and positioning of the bubble based on the camera's position.
// </summary>

public class ThoughtBubbleController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("The camera that the bubble will face.")]
    public Transform cameraTransform;

    private Image _wantedItemIcon;

    public TextMeshPro text;

    private void Start()
    {
        _wantedItemIcon = GetComponentInChildren<Image>();
        gameObject.SetActive(true);
        text.enabled = false;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }

    public void SetWantedItem(Image itemIcon)
    {
        _wantedItemIcon = itemIcon;
    }

    public void HideBubble()
    {
        gameObject.SetActive(false);
    }

    public void ShowBubble()
    {
        gameObject.SetActive(true);
    }

    public void ShowItem()
    {
        _wantedItemIcon.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        _wantedItemIcon.gameObject.SetActive(false);
    }

    public void ShowText()
    {
        text.enabled = true;
    }

    public void HideText()
    {
        text.enabled = false;
    }
}
