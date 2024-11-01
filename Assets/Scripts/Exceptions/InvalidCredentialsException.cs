using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Usuario y/o contraseña incorrecto(s).")
    {
    }
}
