using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base("El usuario ya existe. Prueba con otro nombre de usuario.")
    {
    }
}
