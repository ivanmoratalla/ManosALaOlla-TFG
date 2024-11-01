using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public interface IAuthManager
{
    public Task<bool> TryAutoLogin();

    public Task<bool> SignInWithUsernamePasswordAsync(string username, string password);

    public Task<bool> SignUpWithUsernamePasswordAsync(string username, string password);

    public Task<bool> SignInAnonymouslyAsync();

    public void SignOut();
}
