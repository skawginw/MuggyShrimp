using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string sceneName;

    // Loads the specified scene set in the inspector
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    // Loads the MenuScene
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene"); // Make sure the scene is added to Build Settings
    }

    // Quits the application
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
