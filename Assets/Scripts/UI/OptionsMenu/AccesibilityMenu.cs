using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccesibilityMenu : MonoBehaviour
{
    [SerializeField] private Button goBackButton;
    
    [SerializeField] private Button voiceCommandsButton;
    [SerializeField] private VoiceCommandsMenu voiceCommandsMenu;

    [SerializeField] private Button rebindingButton;
    [SerializeField] private RebindKeyMenu rebindKeyMenu;

    private GameObject previousUI;   

    private void Awake()
    {
        // Configuración de listeners de todos los elemento de la UI
        goBackButton.onClick.AddListener(GoBack);
        voiceCommandsButton.onClick.AddListener(OpenVoiceCommandsMenu);
        rebindingButton.onClick.AddListener(OpenRebindKeysMenu);
    }

    public void OpenAccesibilityMenu(GameObject previousUI)
    {
        this.previousUI = previousUI;
        this.previousUI.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void OpenRebindKeysMenu()
    {
        rebindKeyMenu.OpenVoiceCommandsMenu(this.gameObject);
    }

    private void OpenVoiceCommandsMenu()
    {
        voiceCommandsMenu.OpenVoiceCommandsMenu(this.gameObject);
    }

    private void GoBack()
    {
        this.gameObject.SetActive(false);
        previousUI.SetActive(true);
        previousUI = null;
    }
}
