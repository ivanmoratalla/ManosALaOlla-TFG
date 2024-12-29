using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebindKeyMenu : MonoBehaviour
{
    [SerializeField] private InputServiceAsset inputServiceAssetPlayer1;
    [SerializeField] private InputServiceAsset inputServiceAssetPlayer2;

    
    [SerializeField] private Dropdown playerDropdown;                   // Desplegable para seleccionar jugador

    // Botones de rebinding (compartidos por ambos jugadores)
    [SerializeField] private Button pickObjectButton;
    [SerializeField] private Button releaseObjectButton;
    [SerializeField] private Button cutFoodButton;
    [SerializeField] private Button serveDishButton;

    [SerializeField] private Button applyChangesButton;                 // Botón para aplicar cambios
    [SerializeField] private Button backButton;                         // Botón para volver atrás
    [SerializeField] private Button resetButton;

    private InputServiceAsset currentInputServiceAsset;
    private Dictionary<string, KeyCode> temporaryKeyBindings;           // Aquí se guardará lo que elija el jugador antes de aplicar

    private GameObject previousUI;



    private void Start()
    {
        // Configurar opciones del desplegable
        playerDropdown.ClearOptions();
        playerDropdown.AddOptions(new List<string> { "Jugador 1", "Jugador 2" });
        playerDropdown.onValueChanged.AddListener(OnPlayerSelectionChanged);

        // Asignar callbacks a los botones
        pickObjectButton.onClick.AddListener(() => StartRebind("PickObject"));
        releaseObjectButton.onClick.AddListener(() => StartRebind("ReleaseObject"));
        cutFoodButton.onClick.AddListener(() => StartRebind("CutFood"));
        serveDishButton.onClick.AddListener(() => StartRebind("ServeDish"));

        applyChangesButton.onClick.AddListener(ApplyChanges);
        backButton.onClick.AddListener(GoBack);
        resetButton.onClick.AddListener(ResetKeys);

        // Inicializar configuración temporal y cargar datos del jugador 1
        temporaryKeyBindings = new Dictionary<string, KeyCode>();
        OnPlayerSelectionChanged(0);
    }

    private void OnPlayerSelectionChanged(int selectedIndex)
    {
        // Cambiar el jugador actual según el índice seleccionado
        currentInputServiceAsset = selectedIndex == 0 ? inputServiceAssetPlayer1 : inputServiceAssetPlayer2;

        // Copiar las teclas actuales al diccionario temporal
        temporaryKeyBindings["PickObject"] = currentInputServiceAsset.getPickObjectKey();
        temporaryKeyBindings["ReleaseObject"] = currentInputServiceAsset.getReleaseObjectKey();
        temporaryKeyBindings["CutFood"] = currentInputServiceAsset.getCutFoodKey();
        temporaryKeyBindings["ServeDish"] = currentInputServiceAsset.getServeDishKey();

        // Actualizar las etiquetas de los botones con las teclas temporales
        UpdateKeyDisplay();
        Debug.Log($"Jugador seleccionado: {(selectedIndex == 0 ? "Jugador 1" : "Jugador 2")}");
    }

    private void StartRebind(string actionToRebind)
    {
        Button buttonToUpdate = null;

        switch (actionToRebind)
        {
            case "PickObject": buttonToUpdate = pickObjectButton; break;
            case "ReleaseObject": buttonToUpdate = releaseObjectButton; break;
            case "CutFood": buttonToUpdate = cutFoodButton; break;
            case "ServeDish": buttonToUpdate = serveDishButton; break;
        }

        if (buttonToUpdate != null)
        {
            buttonToUpdate.GetComponentInChildren<Text>().text = "Pulse una tecla...";
            StartCoroutine(WaitForKeyPress(actionToRebind, buttonToUpdate));
        }
    }

    private IEnumerator WaitForKeyPress(string actionToRebind, Button buttonToUpdate)
    {
        while (!Input.anyKeyDown) // Esperar hasta que el jugador pulse una tecla
        {
            yield return null;
        }

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))) // Detectar la tecla pulsada
        {
            if (Input.GetKeyDown(keyCode))
            {
                // Actualizar el diccionario temporal
                temporaryKeyBindings[actionToRebind] = keyCode;

                Debug.Log($"Reasignado {actionToRebind} a {keyCode} temporalmente.");
                break;
            }
        }
        UpdateKeyDisplay();
    }

    private void ApplyChanges()
    {
        foreach (string action in temporaryKeyBindings.Keys)
        {
            currentInputServiceAsset.SetKey(action, temporaryKeyBindings[action]);
        }
        /*
        currentInputServiceAsset.SetPickObjectKey(temporaryKeyBindings["PickObject"]);
        currentInputServiceAsset.SetReleaseObjectKey(temporaryKeyBindings["ReleaseObject"]);
        currentInputServiceAsset.SetCutFoodKey(temporaryKeyBindings["CutFood"]);
        currentInputServiceAsset.SetServeDishKey(temporaryKeyBindings["ServeDish"]);
        */
        Debug.Log($"{(playerDropdown.value == 0 ? "Player 1" : "Player 2")} ha aplicado los cambios.");
    }

    private void ResetKeys()
    {
        currentInputServiceAsset.ResetToDefaults();

        temporaryKeyBindings["PickObject"] = currentInputServiceAsset.getPickObjectKey();
        temporaryKeyBindings["ReleaseObject"] = currentInputServiceAsset.getReleaseObjectKey();
        temporaryKeyBindings["CutFood"] = currentInputServiceAsset.getCutFoodKey();
        temporaryKeyBindings["ServeDish"] = currentInputServiceAsset.getServeDishKey();

        UpdateKeyDisplay();

        Debug.Log($"{(playerDropdown.value == 0 ? "Player 1" : "Player 2")} ha sido restablecido a los valores por defecto.");
    }

    public void OpenVoiceCommandsMenu(GameObject previousUI)
    {
        this.previousUI = previousUI;
        this.previousUI.SetActive(false);
        this.gameObject.SetActive(true);
    }

    private void GoBack()
    {
        OnPlayerSelectionChanged(currentInputServiceAsset.getPlayerId() - 1);   // Como no se aplican los cambios, se ponen los valores guardados

        this.gameObject.SetActive(false);
        previousUI.SetActive(true);
        previousUI = null;
    }

    private void UpdateKeyDisplay()
    {
        pickObjectButton.GetComponentInChildren<Text>().text = temporaryKeyBindings["PickObject"].ToString();
        releaseObjectButton.GetComponentInChildren<Text>().text = temporaryKeyBindings["ReleaseObject"].ToString();
        cutFoodButton.GetComponentInChildren<Text>().text = temporaryKeyBindings["CutFood"].ToString();
        serveDishButton.GetComponentInChildren<Text>().text = temporaryKeyBindings["ServeDish"].ToString();
    }
}

