using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    public List<string> loreToRead;
    private int currentIndex;
    private TypewriterEffect twe;
    private ReadTextFile rtf;

    void Start()
    {
        currentIndex = 0;
        rtf = new ReadTextFile();
        twe = GetComponent<TypewriterEffect>();

        GameEventsManager.instance.biomeEvents.onFireBiomeEnter += EnterBiomeStory;
        GameEventsManager.instance.biomeEvents.onThunderBiomeEnter += EnterBiomeStory;
        GameEventsManager.instance.biomeEvents.onIceBiomeEnter += EnterBiomeStory;

        // NextLoreText();
    }

    private void OnDisable()
    {
        GameEventsManager.instance.biomeEvents.onFireBiomeEnter -= EnterBiomeStory;
        GameEventsManager.instance.biomeEvents.onThunderBiomeEnter -= EnterBiomeStory;
        GameEventsManager.instance.biomeEvents.onIceBiomeEnter -= EnterBiomeStory;
    }

    private void EnterBiomeStory(string file)
    {
        string[] lines = rtf.ReadFileLinesTab(file);

        StartCoroutine(ReadLoreCoroutine(lines));
    }

    private void NextLoreText()
    {
        while (currentIndex < loreToRead.Count)
        {
            string[] lines = rtf.ReadFileLinesTab(loreToRead[currentIndex]);
            currentIndex++;

            StartCoroutine(ReadLoreCoroutine(lines));
        }
    }

    private IEnumerator ReadLoreCoroutine(string[] lines)
    {
        foreach (var line in lines)
        {
            yield return StartCoroutine(twe.ShowText(line));
        }
    }
}
