using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public Text nameText = null;
    [SerializeField] private Button logoutButton = null;


    private IAuthManager authManager;
    [SerializeField] private MenuHandler menuHandler;


    private void Awake()
    {
        authManager = new AuthManager();                        // Instancia de la clase encargada de los métodos de autenticación

        logoutButton.onClick.AddListener(SignOut);                     //Inicializar el botón

    }

    private void SignOut()
    {
        authManager.SignOut();
        menuHandler.ShowAuthMenu();
    }
}
