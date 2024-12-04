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
        pickObjectButton.onClick.AddListener(() => StartRebind(nameof(inputServiceAsset.PickObject)));
        releaseObjectButton.onClick.AddListener(() => StartRebind(nameof(inputServiceAsset.ReleaseObject)));
        cutFoodButton.onClick.AddListener(() => StartRebind(nameof(inputServiceAsset.CutFood)));
        serveDishButton.onClick.AddListener(() => StartRebind(nameof(inputServiceAsset.ServeDish)));

        UpdateKeyDisplay();
    }

    private void StartRebind(string action)
    {

        Debug.Log(action);

        keyToRebind = action;
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        // Esperar a que el jugador pulse una tecla
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                inputServiceAsset.RebindKey(keyToRebind, keyCode);
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

