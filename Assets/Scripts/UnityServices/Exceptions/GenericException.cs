using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericException : Exception
{
    public GenericException() : base("Se ha producido un error. Vuelve a intentarlo.")
    {
    }
}
