using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public Text nameText = null;
    [SerializeField] private Button logoutButton = null;
    [SerializeField] private Button playButton = null;


    private IAuthService authManager;
    [SerializeField] private MenuHandler menuHandler;


    private void Awake()
    {
        authManager = new AuthService();                        // Instancia de la clase encargada de los m�todos de autenticaci�n

        logoutButton.onClick.AddListener(SignOut);                     //Inicializar el bot�n
        playButton.onClick.AddListener(Play);

    }

    private void SignOut()
    {
        authManager.SignOut();
        menuHandler.ShowAuthMenu();
    }

    private void Play()
    {
        SceneManager.LoadScene("LevelsMenu");
        Debug.Log("Cargando escena de selecci�n de niveles");
    }
}
