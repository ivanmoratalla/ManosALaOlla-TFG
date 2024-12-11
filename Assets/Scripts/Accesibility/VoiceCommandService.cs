using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceCommandService : MonoBehaviour
{
    public static VoiceCommandService Instance { get; private set; }                    // Singleton

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> voiceCommands;

    public int activePlayer = -1;                                                      // Ningún jugador con comandos de voz por defecto
    private const string PlayerPrefKey = "ActiveVoicePlayer";                           // Clave para guardar el jugador activo en PlayerPrefs

    // Eventos para notificar los comandos por voz
    public static EventHandler<int> OnPickUpObject;
    public static EventHandler<int> OnReleaseObject;
    public static EventHandler<int> OnServeDish;
    public static EventHandler<int> OnCutIngredient;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void EnableVoiceCommands(int playerId)
    {
        Debug.Log("PERMITIENDO VOICE COMMANDS");
        if (IsEnabled() && activePlayer == playerId)
        {
            Debug.Log($"Comandos de voz ya habilitados para el jugador {playerId}.");
            return;
        }

        activePlayer = playerId;
        InitializeRecognizer();
        keywordRecognizer?.Start();

        Debug.Log($"Comandos de voz habilitados para el jugador {playerId}.");

        SaveSettings();
    }

    public void DisableVoiceCommands()
    {
        if (!IsEnabled())
        {
            Debug.Log("No hay comandos de voz habilitados para desactivar.");
            return;
        }

        keywordRecognizer?.Stop();
        keywordRecognizer?.Dispose();
        keywordRecognizer = null;

        Debug.Log($"Comandos de voz desactivados para el jugador {activePlayer}.");
        activePlayer = -1;

        SaveSettings();
    }

    private void InitializeRecognizer()
    {
        if (keywordRecognizer != null) return;

        voiceCommands = new Dictionary<string, System.Action>
        {
            { "coger", () => OnPickUpObject?.Invoke(this, activePlayer) },
            { "soltar", () => OnReleaseObject?.Invoke(this, activePlayer) },
            { "servir", () => OnServeDish?.Invoke(this, activePlayer) },
            { "cortar", () => OnCutIngredient?.Invoke(this, activePlayer) }
        };

        keywordRecognizer = new KeywordRecognizer(voiceCommands.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnCommandRecognized;
    }

    private void OnCommandRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("FRASE RECONOCIDA");

        if (!IsEnabled()) return;

        if (voiceCommands.TryGetValue(args.text, out var action))
        {
            Debug.Log($"Comando reconocido: {args.text} por el jugador {activePlayer}.");
            action.Invoke();
        }
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKey))
        {
            int savedPlayerId = PlayerPrefs.GetInt(PlayerPrefKey, -1);

            if (savedPlayerId != -1)
            {
                EnableVoiceCommands(savedPlayerId);
                Debug.Log($"Configuración cargada: comandos de voz habilitados para el jugador {savedPlayerId}.");
            }
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(PlayerPrefKey, activePlayer);
        PlayerPrefs.Save();
        Debug.Log($"Configuración guardada: jugador {activePlayer}.");
    }

    public bool IsEnabled()
    {
        return activePlayer != -1;
    }

    public int GetActivePlayer()
    {
        return activePlayer;
    }
}