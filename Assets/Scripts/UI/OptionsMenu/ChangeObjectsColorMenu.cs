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

    [SerializeField] private ColorManager colorManager;

    private Type currentType;
    private Color selectedColor;

    private void Start()
    {
        typeDropdown.ClearOptions();
        typeDropdown.AddOptions(new List<string> { "Food", "KitchenAppliance", "Plate", "Counter" });

        colorButton.onClick.AddListener(OpenColorPicker);
        applyChangesButton.onClick.AddListener(ApplyChanges);
    }

    private void OpenColorPicker()
    {
        ColorPicker.Create(colorManager.GetAlternativeColorForType(typeof(Counter)), "Elija un color", null, ColorChosen, true);
    }

    private void ColorChosen(Color finishedColor)
    {
        Debug.Log("Finished color");
        selectedColor = finishedColor;
    }

    private void ApplyChanges()
    {
        colorManager.SetAlternativeColorForType(typeof(Counter), selectedColor);
    }
}
