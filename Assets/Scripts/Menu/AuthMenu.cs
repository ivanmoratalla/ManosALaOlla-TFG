using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class AuthMenu : MonoBehaviour
{

    [SerializeField] private InputField usernameInput = null;   // Campo para que el usuario introduzca su nombre de usuario
    [SerializeField] private InputField passwordInput = null;   // Campo para que el usuario introduzca su contraseña
    [SerializeField] private Button signinButton = null;        // Botón para el inicio de sesión
    [SerializeField] private Button signupButton = null;        // Botón para registrarse
    [SerializeField] private Button anonymousButton = null;     // Botón para inicio de sesión como invitado

    private IAuthManager authManager;
    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        authManager = new AuthManager();                        // Instancia de la clase encargada de los métodos de autenticación
        
        //Inicializar los botones con las acciones que deben de hacer
        signinButton.onClick.AddListener(SignIn);
        signupButton.onClick.AddListener(SignUp);
        anonymousButton.onClick.AddListener(AnonymousSignIn);
    }

    private void Start()
    {
        usernameInput.text = "";
        passwordInput.text = "";
    }

    // Método para el botón de iniciar sesión como invitado
    private void AnonymousSignIn()
    {
        authManager.SignInAnonymouslyAsync();
    }

    // Método para el botón de inicio de sesión
    private async void SignIn()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            if(await authManager.SignInWithUsernamePasswordAsync(user, pass)) {
                menuHandler.ShowMainMenu();
            }
            else
            {
                menuHandler.ShowError("No se ha podido iniciar sesión");
            }
        }
    }

    // Método para el botón de registrarse
    private async void SignUp()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            if (IsPasswordValid(pass))
            {
                if(await authManager.SignUpWithUsernamePasswordAsync(user, pass))
                {
                    menuHandler.ShowMainMenu();
                }
                else
                {
                    menuHandler.ShowError("No se ha podido registrar el usuario");
                }
            }
            else
            {
                Debug.Log("La contraseña no es válida");
                //ErrorMenu panel = (ErrorMenu)PanelManager.GetSingleton("error");
                //panel.Open(ErrorMenu.Action.None, "La contraseña debe contener al menos una mayúscula, una minúscula, un dígito y un símbolo. Mínimo de 8 caracteres y maximo de 30.", "OK");
                menuHandler.ShowError("La contraseña debe contener al menos una mayúscula, una minúscula, un dígito y un símbolo. Mínimo de 8 caracteres y maximo de 30.");
            }
        }
    }
    
    // Método para comprobar si una contraseña es válida al registrarse (según los criterios de Unity Authentication)
    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 30)
        {
            return false;
        }
        
        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;
        bool hasSymbol = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasUppercase = true;
            }
            else if (char.IsLower(c))
            {
                hasLowercase = true;
            }
            else if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (!char.IsLetterOrDigit(c))
            {
                hasSymbol = true;
            }
        }
        return hasUppercase && hasLowercase && hasDigit && hasSymbol;
    }
    
}