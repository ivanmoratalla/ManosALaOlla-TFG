using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;

public class AuthService: IAuthService
{
    public AuthService()
    {
        InitializeUnityServices();
    }

    private async void InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Se ha producido un error al inicializar los servicios de Unity: " + e.Message);
        }
    }

    // M�todo para comprobar si el usuaro ya est� autenticado o si existe un token de sesi�n previo (con el que iniciar la sesi�n sin que el usuario tenga que hacer nada ni introducir ning�n dato)
    public async Task<bool> TryAutoLogin()
    {
        if (AuthenticationService.Instance.IsSignedIn)                              // Se comprueba si el usuario ya est� autenticado (no hay que hacer nada m�s en ese caso)
        {
            Debug.Log("El usuario ya est� autenticado");
            return true;
        }

        if (AuthenticationService.Instance.SessionTokenExists)                      // Si existe un token de sesi�n, se intenta iniciar sesi�n de manera autom�tica
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Se ha hecho login autom�tico del usuario con su anterior token de sesi�nc");
                return true;
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"Fallo al hacer login autom�tico: {e.Message}");
            }
        }

        return false;                                                               // Se devuelve fals si no se ha podido iniciar sesi�n autom�ticamente
    }

    // M�todo para el inicio de sesi�n con usuario y contrase�a
    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("Inicio de sesi�n correcto");
        }
        catch (System.Exception e)
        {
            HandleExceptionError(e);
        }
    }

    // M�todo para el registro con usuario y contrase�a
    public async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("Registro correcto.");
        }
        catch (System.Exception e)
        {
            HandleExceptionError(e);
            Debug.LogError($"Registro incorrecto: {e.Message}");
        }
        
    }

    // M�todo para el inicio de sesi�n como invitado
    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Inicio de sesi�n an�nimo correcto");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al iniciar sesi�n de manera an�nima: {e.Message}");
            throw new SignInAnonymouslyException();
        }
    }

    // M�todo para cerrar la sesi�n
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();                         // Al cerrar la sesi�n se elimina el token, para que al volver a iniciar la aplicaci�n no se inicie sesi�n autom�ticamente
    }

    // M�todo para comprobar algunas excepciones concretas. Si no, se lanza una excepci�n gen�rica
    private void HandleExceptionError(System.Exception e)
    {
        if (e.Message.Contains("username already exists")) {                        // Comprobaci�n de si es una excepci�n de que ya existe el nombre de usuario
            Debug.LogError($"El usuario ya existe: {e.Message}");
            throw new UserAlreadyExistsException();
        }
        else if(e.Message.Contains("Invalid username or password"))                 // Comprobaci�n de si es una excepci�n de que el usuario o contrase�a introducidos son incorrectos
        {
            Debug.LogError($"Usuario y/o contrase�a incorrectos: {e.Message}");
            throw new InvalidCredentialsException();
        }
        else                                                                        // Excepci�n gen�rica que se lanza
        {
            Debug.Log($"Se ha producido un error: {e.Message}");
            throw new GenericException();
        }
    }
}