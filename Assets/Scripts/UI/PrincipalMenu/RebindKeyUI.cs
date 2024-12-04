using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebindKeyUI : MonoBehaviour
{
    [SerializeField] private InputServiceAsset inputServiceAsset;

    [SerializeField] private Button pickObjectButton;
    [SerializeField] private Button releaseObjectButton;
    [SerializeField] private Button cutFoodButton;
    [SerializeField] private Button serveDishButton;

    private string keyToRebind;

    private void Start()
    {
        // Asignar callbacks a los botones
        pickObjectButton.onClick.AddListener(() => StartRebind("PickObject", pickObjectButton));
        releaseObjectButton.onClick.AddListener(() => StartRebind("ReleaseObject", releaseObjectButton));
        cutFoodButton.onClick.AddListener(() => StartRebind("CutFood", cutFoodButton));
        serveDishButton.onClick.AddListener(() => StartRebind("ServeDish", serveDishButton));

        UpdateKeyDisplay();
    }

    private void StartRebind(string action, Button pressedButton)
    {
        keyToRebind = action;

        pressedButton.GetComponentInChildren<Text>().text = "Pulse una tecla...";

        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKeyDown)                                               // Se espera hasta que el jugador pulse una tecla
        {
            yield return null;
        }

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))     // Se busca la tecla que el jugaodr ha pulsado
        {
            if (Input.GetKeyDown(keyCode))
            {
                switch (keyToRebind)
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

    private void UpdateKeyDisplay()
    {
        pickObjectButton.GetComponentInChildren<Text>().text = inputServiceAsset.getPickObjectKey().ToString();
        releaseObjectButton.GetComponentInChildren<Text>().text = inputServiceAsset.getReleaseObjectKey().ToString();
        cutFoodButton.GetComponentInChildren<Text>().text = inputServiceAsset.getCutFoodKey().ToString();
        serveDishButton.GetComponentInChildren<Text>().text = inputServiceAsset.getServeDishKey().ToString();
    }
}

