using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VoiceCommandsMenu : MonoBehaviour
{
    [SerializeField] private Toggle voiceCommandsToggle; // Toggle para activar/desactivar comandos de voz
    [SerializeField] private Dropdown playerDropdown;    // Dropdown para seleccionar el jugador activo
    [SerializeField] private Button applyButton;         // Botón para aplicar cambios
    [SerializeField] private Button backButton;          // Botón para regresar al menú principal

    private GameObject previousUI;

    private void Start()
    {
        InitializeMenu();  
    }

    private void InitializeMenu()
    {
        voiceCommandsToggle.onValueChanged.AddListener(OnVoiceCommandsToggleChanged);
        applyButton.onClick.AddListener(ApplySettings);
        backButton.onClick.AddListener(GoBack);

        if (playerDropdown != null)
        {
            playerDropdown.onValueChanged.AddListener(OnPlayerSelectionChanged);
        }

        // Cargar configuración inicial
        var voiceService = VoiceCommandService.Instance;
        if (voiceService != null)
        {
            // Configurar estado inicial del toggle
            voiceCommandsToggle.isOn = voiceService.IsEnabled();

            // Configurar dropdown con jugadores (suponiendo dos jugadores)
            playerDropdown.ClearOptions();
            playerDropdown.AddOptions(new System.Collections.Generic.List<string> { "None", "Player 1", "Player 2" });

            // Seleccionar jugador activo
            int activePlayer = voiceService.GetActivePlayer();
            playerDropdown.value = activePlayer == -1 ? 0 : activePlayer;
        }
    }

    public void OpenVoiceCommandsMenu(GameObject previousUI)
    {
        this.previousUI = previousUI;
        this.previousUI.SetActive(false);
        this.gameObject.SetActive(true);
    }

    // Método para desactivar o activar el dropdown en función del valor del toggle
    private void OnVoiceCommandsToggleChanged(bool isOn)                        
    {
        playerDropdown.interactable = isOn;

        if (!isOn)
        {
            playerDropdown.value = 0;
        }
    }

    private void OnPlayerSelectionChanged(int selectedIndex)
    {
        Debug.Log($"Jugador seleccionado: {selectedIndex} (None = 0)");
    }

    private void ApplySettings()
    {
        var voiceService = VoiceCommandService.Instance;

        if (voiceService != null)
        {
            if (voiceCommandsToggle.isOn)
            {
                // Activar comandos de voz para el jugador seleccionado
                int selectedPlayer = playerDropdown.value;
                voiceService.EnableVoiceCommands(selectedPlayer);
                Debug.Log($"Comandos de voz activados para el jugador {selectedPlayer}.");
            }
            else
            {
                // Desactivar comandos de voz
                voiceService.DisableVoiceCommands();
                Debug.Log("Comandos de voz desactivados.");
            }
        }
    }

    private void GoBack()
    {
        this.gameObject.SetActive(false);
        previousUI.SetActive(true);
        previousUI = null;
    }
}

