using UnityEngine;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public Button exitButton; // Кнопка "Выход"
    public Button playButton; // Кнопка "Играть"

    void Start()
    {
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ShowPlayButton);
        }
    }

    void ShowPlayButton()
    {
        if (playButton != null)
        {
            playButton.gameObject.SetActive(true);
        }
    }
}