using UnityEngine;

public class SettingPopup : MonoBehaviour
{
    public void ClosePopup()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
