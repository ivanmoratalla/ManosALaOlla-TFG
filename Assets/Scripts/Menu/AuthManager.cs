using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;

public class AuthManager: IAuthManager
{
    public AuthManager()
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
                Debug.LogError($"Auto-login failed: {e.Message}");
            }
        }

        return false;                                                               // Se devuelve fals si no se ha podido iniciar sesi�n autom�ticamente
    }

    public async Task<bool> SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            Debug.Log("Inicio de sesi�n correcto");
            return true;
        }
        catch (AuthenticationException e)
        {
            Debug.LogError($"Inicio de sesi�n incorrecto: {e.Message}");
            return false;
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"Inicio de sesi�n incorrecto: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("Registro correcto.");
            return true;
        }
        catch (AuthenticationException e)
        {
            Debug.LogError($"Registro incorrecto: {e.Message}");
            return false;
        }
        catch (RequestFailedException e)
        {
            Debug.LogError($"Registro incorrecto: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Inicio de sesi�n an�nimo correcto");
            return true;
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            return false;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();     // Al cerrar la sesi�n se elimina el token, para que al volver a iniciar la aplicaci�n no se inicie sesi�n autom�ticamente
    }
}