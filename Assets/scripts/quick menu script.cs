using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting; // Add this import for Visual Scripting

public class UIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public bool paused = false;

    void Start()
    {
        UpdatePausedState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainMenu.activeSelf)
            {
                DisableAllMenus();
            }
            else
            {
                ShowMainMenu();
            }
        }
    }

    private void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        UpdatePausedState();
    }

    public void DisableAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        UpdatePausedState();
    }

    public void ToggleMenus()
    {
        bool isSettingsActive = settingsMenu.activeSelf;

        if (!isSettingsActive)
        {
            settingsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        UpdatePausedState();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    private void UpdatePausedState()
    {
        if (mainMenu.activeSelf || settingsMenu.activeSelf)
        {
            paused = true;
        }
        else
        {
            paused = false;
        }
    }
}
