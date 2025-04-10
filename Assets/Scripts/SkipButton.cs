using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipButton : MonoBehaviour
{
    [Header("Scene to Skip To")]
    public string sceneToLoad = "Stage_05"; // Change to your test scene

    public void SkipToScene()
    {
        Debug.Log($"[SKIP] Skipping to scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
