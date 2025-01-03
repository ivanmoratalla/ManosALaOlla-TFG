using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInAnonymouslyException : Exception
{
    public SignInAnonymouslyException() : base("Se ha producido un error al intentar iniciar sesión de manera anónima.")
    {
    }
}
