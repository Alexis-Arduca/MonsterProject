using System;
using UnityEngine;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    public string popUpMessage;
    private GameObject objectPopUp;
    private TMP_Text messagePopUp;

    // Start is called before the first frame update
    void Awake()
    {
        objectPopUp = GameObject.Find("MessagePopUp");
        messagePopUp = objectPopUp.GetComponent<TMP_Text>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (objectPopUp != null)
            {
                messagePopUp.text = popUpMessage;
            }
            else
            {
                messagePopUp.text = "";
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (objectPopUp != null)
            {
                messagePopUp.text = "";
            }
        }
    }
}
