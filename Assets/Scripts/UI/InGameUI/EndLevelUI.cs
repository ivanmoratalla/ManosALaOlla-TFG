using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelUI : MonoBehaviour
{
    [SerializeField] private GameObject endLevelPanel;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text neededScoreText;
    [SerializeField] private Text levelPassedText;

    private void OnEnable()
    {
        Level.OnGameOver += ShowEndLevelPanel;
    }

    private void OnDisable()
    {
        Level.OnGameOver -= ShowEndLevelPanel;
    }

    private void Start()
    {
        endLevelPanel.SetActive(false);

        mainMenuButton.onClick.AddListener(() => LoadMainMenu());
        levelSelectionButton.onClick.AddListener(() => LoadLevelsMenu());
        replayButton.onClick.AddListener(() => ReplayLevel());
        nextLevelButton.onClick.AddListener(() => LoadNextLevel());
    }

    private void ShowEndLevelPanel(object sender, KeyValuePair<int, int> values)
    {
        Time.timeScale = 0f;

        int finalStars = values.Key;
        int neededStars = values.Value;

        if(finalStars >= neededStars)
        {
            nextLevelButton.interactable = true;
            levelPassedText.gameObject.SetActive(true);
        }
        else
        {
            nextLevelButton.interactable = false;
            levelPassedText.gameObject.SetActive(false);
        }

        endLevelPanel.SetActive(true);
        scoreText.text = "Puntuación obtenida: " + finalStars;
        neededScoreText.text = "Puntuación necesaria: " + neededStars;
    }

    private void LoadMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Menu");
    }

    private void LoadLevelsMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("LevelsMenu");
    }

    private void ReplayLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;

        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            Debug.Log("No hay más niveles disponibles.");
            SceneManager.LoadScene("LevelsMenu");
        }
    }
}
