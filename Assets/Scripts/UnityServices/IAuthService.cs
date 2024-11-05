using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public interface IAuthService
{
    public Task<bool> TryAutoLogin();

    public Task SignInWithUsernamePasswordAsync(string username, string password);

    public Task SignUpWithUsernamePasswordAsync(string username, string password);

    public Task SignInAnonymouslyAsync();

    public void SignOut();
}
