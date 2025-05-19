using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePopup : MonoBehaviour
{
    public string popUpMessage;
    private GameObject objectPopUp;
    private TMPro.TMP_Text messagePopUp;

    // Start is called before the first frame update
    void Awake()
    {
        objectPopUp = GameObject.Find("MessagePopUp");
        messagePopUp = objectPopUp.GetComponent<TMP_Text>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectPopUp != null)
            {
                messagePopUp.text = "";
            }
        }
    }
}
