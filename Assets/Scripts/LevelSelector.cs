using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText.SetText(level.ToString());
    }

    public void OpenScene()
    {
        LevelManager.currentLevel = level;
        SceneManager.LoadScene("Game");
    }
}
