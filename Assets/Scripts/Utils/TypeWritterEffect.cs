using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    private string fullText;
    public float typingSpeed = 0.05f;
    public float moveDuration = 2f;
    public float moveDistance = 50f;

    private string currentText = "";

    public void Start()
    {
        
    }

    public void StartText(string msg)
    {
        this.fullText = msg;
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
        StartCoroutine(MoveTextUp());
    }

    IEnumerator MoveTextUp()
    {
        Vector3 originalPosition = textComponent.rectTransform.anchoredPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, moveDistance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            textComponent.rectTransform.anchoredPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textComponent.rectTransform.anchoredPosition = targetPosition;
    }
}
