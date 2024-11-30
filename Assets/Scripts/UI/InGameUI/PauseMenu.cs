using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    [SerializeField] private Button optionsButton;
    [SerializeField] private Button goBackButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button reloadButton;


    private void Awake()
    {
        optionsButton.onClick.AddListener(OpenOptionsMenu);
        goBackButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(ResumeGame);
        reloadButton.onClick.AddListener(RestartLevel);
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;
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
        OptionsMenu.Instance.OpenOptionsMenu();
    }

    private void RestartLevel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

}
