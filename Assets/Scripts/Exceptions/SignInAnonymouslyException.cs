using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInAnonymouslyException : Exception
{
    public SignInAnonymouslyException() : base("Se ha producido un error al intentar iniciar sesi�n de manera an�nima.")
    {
    }
}
