using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubbleController : MonoBehaviour
{
    [Header("Camera")]
    [Tooltip("The camera that the bubble will face.")]
    public Transform cameraTransform;

    [Header("Item")]
    private Image _wantedItemIcon;

    [Header("Text")]
    public TextMeshPro text;

    private void Start()
    {
        _wantedItemIcon = GetComponentInChildren<Image>();
        // Set the bubble to be inactive at the start
        gameObject.SetActive(true);
        text.enabled = false;
    }

    private void LateUpdate()
    {
        // Make the bubble always face the camera
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
