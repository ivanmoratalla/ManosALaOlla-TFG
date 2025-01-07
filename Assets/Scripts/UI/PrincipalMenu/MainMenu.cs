using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button logoutButton = null;
    [SerializeField] private Button playButton = null;
    [SerializeField] private Button optionsButton = null;

    private IAuthService authManager;
    [SerializeField] private MenuHandler menuHandler;


    private void Awake()
    {
        authManager = new AuthService();                        // Instancia de la clase encargada de los métodos de autenticación

        logoutButton.onClick.AddListener(SignOut);                     //Inicializar el botón
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(OpenOptionsMenu);

    }

    private void SignOut()
    {
        authManager.SignOut();
        menuHandler.ShowAuthMenu();
    }

    private void Play()
    {
        SceneManager.LoadScene("LevelsMenu");
        Debug.Log("Cargando escena de selección de niveles");
    }

    private void OpenOptionsMenu()
    {
        menuHandler.ShowOptionsMenu();
    }
}
