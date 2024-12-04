using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebindKeyUI : MonoBehaviour
{
    [SerializeField] private InputServiceAsset inputServiceAssetPlayer1;
    [SerializeField] private InputServiceAsset inputServiceAssetPlayer2;

    // Botones de rebinding para el jugador 1
    [SerializeField] private Button pickObjectButton_Player1;
    [SerializeField] private Button releaseObjectButton_Player1;
    [SerializeField] private Button cutFoodButton_Player1;
    [SerializeField] private Button serveDishButton_Player1;

    // Botones de rebinding para el jugador 2
    [SerializeField] private Button pickObjectButton_Player2;
    [SerializeField] private Button releaseObjectButton_Player2;
    [SerializeField] private Button cutFoodButton_Player2;
    [SerializeField] private Button serveDishButton_Player2;

    // Botones para resetear las teclas a los valores por defecto
    [SerializeField] private Button resetKeysButton_Player1;
    [SerializeField] private Button resetKeysButton_Player2;


    private string keyToRebind;

    private void Start()
    {
        // Asignar callbacks a los botones del jugador 1
        pickObjectButton_Player1.onClick.AddListener(() => StartRebind("PickObject", inputServiceAssetPlayer1, pickObjectButton_Player1));
        releaseObjectButton_Player1.onClick.AddListener(() => StartRebind("ReleaseObject", inputServiceAssetPlayer1, releaseObjectButton_Player1));
        cutFoodButton_Player1.onClick.AddListener(() => StartRebind("CutFood", inputServiceAssetPlayer1, cutFoodButton_Player1));
        serveDishButton_Player1.onClick.AddListener(() => StartRebind("ServeDish", inputServiceAssetPlayer1, serveDishButton_Player1));

        // Asignar callbacks a los botones del jugador 2
        pickObjectButton_Player2.onClick.AddListener(() => StartRebind("PickObject", inputServiceAssetPlayer2, pickObjectButton_Player2));
        releaseObjectButton_Player2.onClick.AddListener(() => StartRebind("ReleaseObject", inputServiceAssetPlayer2, releaseObjectButton_Player2));
        cutFoodButton_Player2.onClick.AddListener(() => StartRebind("CutFood", inputServiceAssetPlayer2, cutFoodButton_Player2));
        serveDishButton_Player2.onClick.AddListener(() => StartRebind("ServeDish", inputServiceAssetPlayer2, serveDishButton_Player2));

        // Asignar botones de resetear configuración
        resetKeysButton_Player1.onClick.AddListener(() => ResetKeys(inputServiceAssetPlayer1));
        resetKeysButton_Player2.onClick.AddListener(() => ResetKeys(inputServiceAssetPlayer2));

        UpdateKeyDisplay();
    }

    private void StartRebind(string actionToRebind, InputServiceAsset inputServiceAsset, Button pressedButton)
    {
        pressedButton.GetComponentInChildren<Text>().text = "Pulse una tecla...";

        StartCoroutine(WaitForKeyPress(actionToRebind, inputServiceAsset));
    }

    private IEnumerator WaitForKeyPress(string actionToRebind, InputServiceAsset inputServiceAsset)
    {
        while (!Input.anyKeyDown)                                               // Se espera hasta que el jugador pulse una tecla
        {
            yield return null;
        }

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))     // Se busca la tecla que el jugaodr ha pulsado
        {
            if (Input.GetKeyDown(keyCode))
            {
                switch (actionToRebind)
                {
                    case "PickObject":
                        inputServiceAsset.SetPickObjectKey(keyCode);
                        break;
                    case "ReleaseObject":
                        inputServiceAsset.SetReleaseObjectKey(keyCode);
                        break;
                    case "CutFood":
                        inputServiceAsset.SetCutFoodKey(keyCode);
                        break;
                    case "ServeDish":
                        inputServiceAsset.SetServeDishKey(keyCode);
                        break;
                }

                Debug.Log($"Reasignado {keyToRebind} a {keyCode}");
                break;
            }
        }
        UpdateKeyDisplay();
    }

    private void ResetKeys(InputServiceAsset inputServiceAsset)
    {
        inputServiceAsset.ResetToDefaults();
        UpdateKeyDisplay();
        Debug.Log($"{inputServiceAsset.name} ha sido restablecido a los valores por defecto.");
    }

    private void UpdateKeyDisplay()
    {
        // Se actualizan las etiquetas de los botones del jugador 1
        pickObjectButton_Player1.GetComponentInChildren<Text>().text = inputServiceAssetPlayer1.getPickObjectKey().ToString();
        releaseObjectButton_Player1.GetComponentInChildren<Text>().text = inputServiceAssetPlayer1.getReleaseObjectKey().ToString();
        cutFoodButton_Player1.GetComponentInChildren<Text>().text = inputServiceAssetPlayer1.getCutFoodKey().ToString();
        serveDishButton_Player1.GetComponentInChildren<Text>().text = inputServiceAssetPlayer1.getServeDishKey().ToString();

        // Se actualizan las etiquetas de los botones del jugador 2
        pickObjectButton_Player2.GetComponentInChildren<Text>().text = inputServiceAssetPlayer2.getPickObjectKey().ToString();
        releaseObjectButton_Player2.GetComponentInChildren<Text>().text = inputServiceAssetPlayer2.getReleaseObjectKey().ToString();
        cutFoodButton_Player2.GetComponentInChildren<Text>().text = inputServiceAssetPlayer2.getCutFoodKey().ToString();
        serveDishButton_Player2.GetComponentInChildren<Text>().text = inputServiceAssetPlayer2.getServeDishKey().ToString();
    }
}

