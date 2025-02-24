using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // Public string field to store the scene name for each button
    public string sceneName;

    // This function will be assigned to the button's OnClick event
    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Load the scene by name
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty!");
        }
    }
}
