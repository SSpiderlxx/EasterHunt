using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private string gameSceneA;
    [SerializeField] private string gameSceneB;

    public void LoadScene()
    {
        if (string.IsNullOrWhiteSpace(gameSceneA) || string.IsNullOrWhiteSpace(gameSceneB))
        {
            Debug.LogWarning("LoadScene needs both gameSceneA and gameSceneB set in the inspector.");
            return;
        }

        string selectedScene = Random.Range(0, 2) == 0 ? gameSceneA : gameSceneB;
        SceneManager.LoadScene(selectedScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
