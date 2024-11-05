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

    // Método para comprobar si el usuaro ya está autenticado o si existe un token de sesión previo (con el que iniciar la sesión sin que el usuario tenga que hacer nada ni introducir ningún dato)
    public async Task<bool> TryAutoLogin()
    {
        if (AuthenticationService.Instance.IsSignedIn)                              // Se comprueba si el usuario ya está autenticado (no hay que hacer nada más en ese caso)
        {
            Debug.Log("El usuario ya está autenticado");
            return true;
        }

        if (AuthenticationService.Instance.SessionTokenExists)                      // Si existe un token de sesión, se intenta iniciar sesión de manera automática
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Se ha hecho login automático del usuario con su anterior token de sesiónc");
                return true;
            }
            catch (AuthenticationException e)
            {
                Debug.LogError($"Fallo al hacer login automático: {e.Message}");
            }
        }

        return false;                                                               // Se devuelve fals si no se ha podido iniciar sesión automáticamente
    }

    // Método para el inicio de sesión con usuario y contraseña
    public async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("Inicio de sesión correcto");
        }
        catch (System.Exception e)
        {
            HandleExceptionError(e);
        }
    }

    // Método para el registro con usuario y contraseña
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

    // Método para el inicio de sesión como invitado
    public async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Inicio de sesión anónimo correcto");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al iniciar sesión de manera anónima: {e.Message}");
            throw new SignInAnonymouslyException();
        }
    }

    // Método para cerrar la sesión
    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();                         // Al cerrar la sesión se elimina el token, para que al volver a iniciar la aplicación no se inicie sesión automáticamente
    }

    // Método para comprobar algunas excepciones concretas. Si no, se lanza una excepción genérica
    private void HandleExceptionError(System.Exception e)
    {
        if (e.Message.Contains("username already exists")) {                        // Comprobación de si es una excepción de que ya existe el nombre de usuario
            Debug.LogError($"El usuario ya existe: {e.Message}");
            throw new UserAlreadyExistsException();
        }
        else if(e.Message.Contains("Invalid username or password"))                 // Comprobación de si es una excepción de que el usuario o contraseña introducidos son incorrectos
        {
            Debug.LogError($"Usuario y/o contraseña incorrectos: {e.Message}");
            throw new InvalidCredentialsException();
        }
        else                                                                        // Excepción genérica que se lanza
        {
            Debug.Log($"Se ha producido un error: {e.Message}");
            throw new GenericException();
        }
    }
}