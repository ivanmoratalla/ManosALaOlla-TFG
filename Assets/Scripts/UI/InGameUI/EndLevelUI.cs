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

        mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        levelSelectionButton.onClick.AddListener(() => SceneManager.LoadScene("LevelsMenu"));
        replayButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        nextLevelButton.onClick.AddListener(() => LoadNextLevel());
    }

    private void ShowEndLevelPanel(object sender, KeyValuePair<int, int> values)
    {
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

    private void LoadNextLevel()
    {
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
