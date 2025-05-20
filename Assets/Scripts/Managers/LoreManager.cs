using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    public List<string> loreToRead;
    private int currentIndex;

    private TypewriterEffect twe;
    private ReadTextFile rtf = new ReadTextFile();

    void Start()
    {
        currentIndex = 0;
        twe = GetComponent<TypewriterEffect>();

        StartCoroutine(ReadLoreCoroutine());
    }

    private IEnumerator ReadLoreCoroutine()
    {
        while (currentIndex < loreToRead.Count)
        {
            string[] lines = rtf.ReadFileLinesTab(loreToRead[currentIndex]);
            currentIndex++;

            foreach (var line in lines)
            {
                yield return StartCoroutine(twe.ShowText(line));
            }
        }
    }
}
