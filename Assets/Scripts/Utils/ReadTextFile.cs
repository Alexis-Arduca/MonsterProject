using System;
using System.IO;
using UnityEngine;

public class ReadTextFile : MonoBehaviour
{
    public string ReadFileAllText(string textFile)
    {
        string text = File.ReadAllText(textFile);

        return text;
    }

    public string[] ReadFileLinesTab(string textFile)
    {
        string[] lines = File.ReadAllLines(textFile);

        return lines;
    }
}
