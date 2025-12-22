using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private string _sceneName; // Имя сцены в Build Settings

    private void Start()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        if (_button != null)
            _button.onClick.AddListener(LoadTargetScene);
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(_sceneName))
        {
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