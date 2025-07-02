using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevelScript : MonoBehaviour
{
    public void ToNextLevel()
    {
        //GetComponent<AudioSource>().Play();
        
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "StartScreenNew")
        {
            Invoke("LoadPlaytest", 0.1f);
        }

        if (currentScene.name == "EndScreenNew")
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
