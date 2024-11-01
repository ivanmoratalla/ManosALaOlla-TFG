using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject errorMenu;
    [SerializeField] private GameObject authMenu;

    private IAuthManager authManager;

    private void Start()
    {
        authManager = new AuthManager();                // Instancia de la clase encargada de los m�todos de autenticaci�n

        InitializeMenus();
    }

    private async void InitializeMenus()
    {
        HideAllMenus();                                 // Se ocultan todos los men�s de manera inicial

        if (await authManager.TryAutoLogin())           // Si se ha podido iniciar sesi�n de manera autom�tica, se va a la pantalla de inicio
        {
            mainMenu.SetActive(true);
        }
        else                                            // Si no se ha podido, se lleva a la pantalla de autenticaci�n
        {
            authMenu.SetActive(true);
        }
    }

    private void HideAllMenus()
    {
        mainMenu.SetActive(false);
        errorMenu.SetActive(false);
        authMenu.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        mainMenu.SetActive(true);
    }

    public void ShowAuthMenu()
    {
        HideAllMenus();
        authMenu.SetActive(true);
    }

    public void ShowError(string message)
    {
        HideAllMenus();

        errorMenu.GetComponent<ErrorMenu>().ShowError(message);
        errorMenu.SetActive(true);
    }
}
