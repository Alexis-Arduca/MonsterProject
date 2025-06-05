using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class House : MonoBehaviour
{
    public Image fadeImage;
    private bool isTransitioning;
    public GameObject player;

    void Start()
    {
        isTransitioning = false;

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            GameEventsManager.instance.loreEvents.OnImportantLoreEvent();
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f, 0.5f));

        player.transform.position = new Vector3(-2.4f, 2.74f, 14.3f);

        yield return StartCoroutine(Fade(1f, 0f, 0.5f));

        isTransitioning = false;
        GameEventsManager.instance.loreEvents.OnImportantLoreEvent();
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, endAlpha);
    }
}