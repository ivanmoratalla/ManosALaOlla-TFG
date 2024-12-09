using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChangeObjectsColorMenu : MonoBehaviour
{
    [SerializeField] private Dropdown typeDropdown;                     // Desplegable para seleccionar el tipo de objeto
    [SerializeField] private Toggle stateToggle;                        // Toggle para activar o desactivar el color alternativo

    [SerializeField] private Button colorButton;                        // Bot�n para elegir color
    [SerializeField] private Button applyChangesButton;                 // Bot�n para aplicar cambios
    [SerializeField] private Button backButton;                         // Bot�n para volver atr�s

    [SerializeField] private ColorManager colorManager;

    private Type currentType;
    private Color selectedColor;

    private void Start()
    {
        colorButton.onClick.AddListener(ChangeColor);
    }

    private void ChangeColor()
    {
        selectedColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        colorManager.SetAlternativeColorForType(typeof(Counter), selectedColor);
    }
}
