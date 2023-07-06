using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject gameName;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject panelSettings;

    public void Play()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void Settings()
    {
        panelSettings.SetActive(true);
        gameName.SetActive(false);
        playButton.SetActive(false);
        settingsButton.SetActive(false);
    }
    
    public void Back()
    {
        panelSettings.SetActive(false);
        gameName.SetActive(true);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
    }
}
