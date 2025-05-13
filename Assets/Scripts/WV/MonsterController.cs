using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    private ThoughtBubbleController _thoughtBubble;
    public Image wantedItem;
    private PickableController _pickableController;

    private void Start()
    {
        _thoughtBubble = GetComponentInChildren<ThoughtBubbleController>();
        _thoughtBubble.ShowBubble();
        _thoughtBubble.SetWantedItem(wantedItem);

    }

    public void Interact(PickableController item)
    {
        if (wantedItem == item.icon)
        {
            _thoughtBubble.HideBubble();
            item.Destroy();
        }
    }
}
