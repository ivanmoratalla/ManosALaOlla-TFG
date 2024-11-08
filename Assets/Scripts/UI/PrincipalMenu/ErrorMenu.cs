using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMenu : MonoBehaviour
{
    [SerializeField] private Text errorText = null;

    [SerializeField] private Button actionButton = null;

    [SerializeField] private MenuHandler menuHandler;


    private void Awake()
    {
        actionButton.onClick.AddListener(ButtonAction);                
    }

    private void ButtonAction()
    {
        menuHandler.ShowAuthMenu();
    }

    public void ShowError(string message)
    {
        errorText.text = message;
    }


}
