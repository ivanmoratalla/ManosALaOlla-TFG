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

    private IAuthService authManager;
    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        authManager = new AuthService();                        // Instancia de la clase encargada de los métodos de autenticación
        
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
    private async void AnonymousSignIn()
    {
        try
        {
            await authManager.SignInAnonymouslyAsync();
            menuHandler.ShowMainMenu();
        }
        catch (SignInAnonymouslyException e)
        {
            menuHandler.ShowError(e.Message);
        }
    }

    // Método para el botón de inicio de sesión
    private async void SignIn()
    {
        string user = usernameInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(user) == false && string.IsNullOrEmpty(pass) == false)
        {
            try
            {
                await authManager.SignInWithUsernamePasswordAsync(user, pass);
                menuHandler.ShowMainMenu();
            }
            catch (UserAlreadyExistsException e)
            {
                menuHandler.ShowError(e.Message);
            }
            catch (InvalidCredentialsException e)
            {
                menuHandler.ShowError(e.Message);
            }
            catch (GenericException e)
            {
                menuHandler.ShowError(e.Message);
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
                try
                {
                    await authManager.SignUpWithUsernamePasswordAsync(user, pass);
                    menuHandler.ShowMainMenu();
                }
                catch (UserAlreadyExistsException e)
                {
                    menuHandler.ShowError(e.Message);
                }
                catch (GenericException e)
                {
                    menuHandler.ShowError(e.Message);
                }  
            }
            else
            {
                Debug.LogWarning("La contraseña no es válida");
                
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