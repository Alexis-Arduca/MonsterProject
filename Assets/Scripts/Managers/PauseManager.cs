using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool pauseState = false;
    private float fixedDeltaTime;

    void Start()
    {
        pausePanel.SetActive(pauseState);

        GameEventsManager.instance.pauseEvents.onPauseButtonPressed += GamePauseState;
    }

    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;   
    }

    private void OnDisable()
    {
        GameEventsManager.instance.pauseEvents.onPauseButtonPressed -= GamePauseState;
    }

    private void GamePauseState()
    {
        pauseState = !pauseState;
        pausePanel.SetActive(pauseState);

        if (pauseState == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
}
