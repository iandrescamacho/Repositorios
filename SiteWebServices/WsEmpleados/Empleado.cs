﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Empleado
/// </summary>
public class Empleado
{
    //#region constructor
    //public Empleado()
    //{
    //    //
    //    // TODO: Agregar aquí la lógica del constructor
    //    //
    //}
    //#endregion constructor

    #region atributos
    public string nombres { get; set; }
    public string apellidos { get; set; }
    public string identificacion { get; set; }
    public string direccion { get; set; }
    public string telefono { get; set; }
    public string celular { get; set; }
    public string email { get; set; }
    #endregion
}