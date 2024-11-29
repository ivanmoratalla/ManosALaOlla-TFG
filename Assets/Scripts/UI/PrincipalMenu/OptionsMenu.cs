using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance { get; private set; }            // Singleton para que se pueda acceder al menú de opciones desde todas las escenas

    [SerializeField] private Button goBackButton = null;
    
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Toggle fullScreenToggle = null;
    [SerializeField] private Dropdown qualityDropdown = null;

    // Atributos para el brillo
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private PostProcessVolume postProcessVolume;
    private ColorGrading colorGrading;

    // Atributos para la resolución
    [SerializeField] private Dropdown resolutionDropdown = null;
    private Resolution[] resolutions;


    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        // Configuración Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Configuración de listeners de todos los elemento de la UI
        goBackButton.onClick.AddListener(GoBack);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        fullScreenToggle.onValueChanged.AddListener(SetFullScreen);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        LoadSettings();
    }

    public void OpenOptionsMenu()
    {
        this.gameObject.SetActive(true);
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

        // Carga la calidad
        int calidad = PlayerPrefs.GetInt("QualityLevel", 4);
        QualitySettings.SetQualityLevel(calidad);
        qualityDropdown.value = calidad;

        // Carga la resolución
        /*if (PlayerPrefs.HasKey("Resolution"))
        {
            int resolution = PlayerPrefs.GetInt("Resolution");

            SetResolution(resolution);
            resolutionDropdown.value = resolution;

        }*/
        //resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", 0);

        InitializeResolutions();
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
    }

    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
        PlayerPrefs.Save();
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }

    private void InitializeResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Se compreba si es la resolución actual
            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutions.Length-1);
    }
}
