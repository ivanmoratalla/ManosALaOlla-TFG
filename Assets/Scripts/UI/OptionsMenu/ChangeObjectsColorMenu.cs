using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeObjectsColorMenu : MonoBehaviour
{
    [SerializeField] private Dropdown typeDropdown;                     // Desplegable para seleccionar el tipo de objeto
    [SerializeField] private Toggle stateToggle;                        // Toggle para activar o desactivar el color alternativo

    [SerializeField] private Button colorButton;                        // Botón para elegir color
    [SerializeField] private Button applyChangesButton;                 // Botón para aplicar cambios
    [SerializeField] private Button backButton;                         // Botón para volver atrás

    private Type currentType;
    private Color selectedColor;
    private GameObject previousUI;


    private void Start()
    {
        typeDropdown.ClearOptions();
        typeDropdown.AddOptions(new List<string> { "Food", "KitchenAppliance", "Plate", "Counter" });
        typeDropdown.onValueChanged.AddListener(OnTypeChanged);

        stateToggle.onValueChanged.AddListener(OnToggleChanged);

        colorButton.onClick.AddListener(OpenColorPicker);
        applyChangesButton.onClick.AddListener(ApplyChanges);
        backButton.onClick.AddListener(GoBack);

        OnTypeChanged(0);   // En un primer momento se pone en el desplegable la primera opción
    }

    private void OnTypeChanged(int dropdownIndex)
    {
        currentType = Type.GetType(typeDropdown.options[dropdownIndex].text);

        if (currentType == null) return;

        stateToggle.isOn = ColorManager.Instance.GetAlternativeStateForType(currentType);

        selectedColor = ColorManager.Instance.GetAlternativeColorForType(currentType);

        colorButton.interactable = stateToggle.isOn;
    }

    private void OnToggleChanged(bool state)
    {
        colorButton.interactable = state;
    }

    private void OpenColorPicker()
    {
        ColorPicker.Create(ColorManager.Instance.GetAlternativeColorForType(currentType), "Elija un color", null, ColorChosen, true);
    }

    private void ColorChosen(Color finishedColor)
    {
        Debug.Log("Finished color");
        selectedColor = finishedColor;
    }

    private void ApplyChanges()
    {
        if (currentType != null)
        {
            ColorManager.Instance.SetAlternativeStateForType(currentType, stateToggle.isOn);

            if (stateToggle.isOn)
            {
                ColorManager.Instance.SetAlternativeColorForType(currentType, selectedColor);
            }
        }

    }

    public void OpenCustomColorsMenu(GameObject previousUI)
    {
        this.previousUI = previousUI;
        this.previousUI.SetActive(false);
        this.gameObject.SetActive(true);
    }

    private void GoBack()
    {
        this.gameObject.SetActive(false);
        previousUI.SetActive(true);
        previousUI = null;
    }
}
