using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class AuthMenu : MonoBehaviour
{

    [SerializeField] private InputField usernameInput = null;   // Campo para que el usuario introduzca su nombre de usuario
    [SerializeField] private InputField passwordInput = null;   // Campo para que el usuario introduzca su contrase�a
    [SerializeField] private Button signinButton = null;        // Bot�n para el inicio de sesi�n
    [SerializeField] private Button signupButton = null;        // Bot�n para registrarse
    [SerializeField] private Button anonymousButton = null;     // Bot�n para inicio de sesi�n como invitado

    private IAuthService authManager;
    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        authManager = new AuthService();                        // Instancia de la clase encargada de los m�todos de autenticaci�n
        
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

    // M�todo para el bot�n de iniciar sesi�n como invitado
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

    // M�todo para el bot�n de inicio de sesi�n
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

    // M�todo para el bot�n de registrarse
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
                Debug.LogWarning("La contrase�a no es v�lida");
                
                menuHandler.ShowError("La contrase�a debe contener al menos una may�scula, una min�scula, un d�gito y un s�mbolo. M�nimo de 8 caracteres y maximo de 30.");
            }
        }
    }
    
    // M�todo para comprobar si una contrase�a es v�lida al registrarse (seg�n los criterios de Unity Authentication)
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