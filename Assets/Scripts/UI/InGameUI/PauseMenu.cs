using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button goBackButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button reloadButton;

    [SerializeField] private OptionsMenu optionsMenu;


    private void Awake()
    {
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        goBackButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(ResumeGame);
        reloadButton.onClick.AddListener(RestartLevel);
    }

    public void PauseGame()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void OpenOptionsMenu()
    {
        optionsMenu.OpenOptionsMenu(this.gameObject);
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

}
