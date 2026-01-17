using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private string _sceneName;
    [SerializeField] private int levelNumber = 1; // ? ÄÎÁÀÂÜ İÒÎ!
    [SerializeField] private GameObject lockIcon; // Èêîíêà çàìêà (îïöèîíàëüíî)

    private void Start()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        // ÏĞÎÂÅĞßÅÌ, ÎÒÊĞÛÒ ËÈ ÓĞÎÂÅÍÜ ? ÄÎÁÀÂÜ İÒÎ!
        if (levelNumber > 1 && LevelProgress.Instance != null)
        {
            bool isUnlocked = LevelProgress.Instance.IsLevelUnlocked(levelNumber);
            _button.interactable = isUnlocked;

            if (lockIcon != null)
                lockIcon.SetActive(!isUnlocked);
        }

        if (_button != null)
            _button.onClick.AddListener(LoadTargetScene);
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(_sceneName))
        {
            // ÄÎÏÎËÍÈÒÅËÜÍÀß ÏĞÎÂÅĞÊÀ ? ÍÀ ÂÑßÊÈÉ ÑËÓ×ÀÉ
            if (levelNumber > 1 && LevelProgress.Instance != null)
            {
                if (!LevelProgress.Instance.IsLevelUnlocked(levelNumber))
                {
                    Debug.Log($"Óğîâåíü {levelNumber} çàáëîêèğîâàí!");
                    return;
                }
            }

            SceneManager.LoadScene(_sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set!");
        }
    }

    private void OnDestroy()
    {
        if (_button != null)
            _button.onClick.RemoveListener(LoadTargetScene);
    }
}
