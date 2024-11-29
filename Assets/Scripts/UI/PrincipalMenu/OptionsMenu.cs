using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button goBackButton = null;
    [SerializeField] private Slider volumeSlider = null;


    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        goBackButton.onClick.AddListener(GoBack);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        LoadSettings();
    }

    private void GoBack()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Menu")
        {
            menuHandler.ShowMainMenu();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private void LoadSettings()
    {
        // Carga del volumen
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        AudioListener.volume = volumeSlider.value;
    }

    public void SetVolume(float volumeValue)
    {
        PlayerPrefs.SetFloat("Volume", volumeValue);
        AudioListener.volume = volumeValue;
    }
}
