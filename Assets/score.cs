using UnityEngine;
using System.Collections;

public class score : MonoBehaviour
{
    public float delay = 2f;

    private void OnEnable()
    {
        // Start the timer when the GameObject is set active
        StartCoroutine(DisableAfterDelay(delay));
    }

    private IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable the GameObject
        gameObject.SetActive(false);
    }
}