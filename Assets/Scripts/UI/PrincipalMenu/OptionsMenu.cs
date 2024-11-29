using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button goBackButton = null;
    
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Toggle fullScreenToggle = null;

    // Atributos para el brillo
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;


    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        goBackButton.onClick.AddListener(GoBack);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);

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

        // Carga el brillo
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        colorGrading.postExposure.value = Mathf.Lerp(-2f, 2f, savedBrightness);
        brightnessSlider.value = savedBrightness;

        // Carga de pantalla completa
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void SetVolume(float volumeValue)
    {
        PlayerPrefs.SetFloat("Volume", volumeValue);
        AudioListener.volume = volumeValue;
        PlayerPrefs.Save();
    }

    private void SetBrightness(float value)
    {
        colorGrading.postExposure.value = Mathf.Lerp(-2f, 2f, value);               // Como trabaja con un rango de -2 a 2, tengo que convertir el valor del slider (que va de 0 a 1) a uno en escala -2 - 2

        // Guardar el valor ajustado.
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

    private void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        //PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        //PlayerPrefs.Save();
    }
}
