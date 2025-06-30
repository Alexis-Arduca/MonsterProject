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

        if (currentScene.name == "EndScreen")
        {
            Invoke("LoadPlaytest", 0.1f);
        }
    }

    public void LoadPlaytest()
    {
        SceneManager.LoadScene("playtest mia");
    }

    public void restartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
