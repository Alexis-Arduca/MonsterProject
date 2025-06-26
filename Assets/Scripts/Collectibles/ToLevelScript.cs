using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevelScript : MonoBehaviour
{
    public void ToNextLevel()
    {
        //GetComponent<AudioSource>().Play();
        
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StartScreen")
        {
            Invoke("LoadPlaytest", 0.1f);
        }
    }

    public void LoadPlaytest()
    {
        SceneManager.LoadScene("Playtest");
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
