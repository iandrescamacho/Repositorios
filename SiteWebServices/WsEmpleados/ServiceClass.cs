using System;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;
using SiteServices;
using Dyetron.Sicap.Datos;

public class ServiceClass : Entidad
{
    private string TokenUser { get; set; }
    private string TokenAdmin { get; set; }

    /// <summary>
    /// Metodo que consulta un empleado por su identificacion
    /// </summary>
    /// <param name="identificacion">numero de identificacion del empleado</param>
    /// <returns>datos XML</returns>
    [WebMethod]
    public Empleado[] getEmpleadoXML(string identificacion)
    {
        Empleado[] objEmpleado = new Empleado[]
        {
            new Empleado()
            {
                nombres="ivan"
            }

        };
        return objEmpleado;
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string getEmpleadoJSON(string token, string identificacion)
    {
        DataTable dt = null;
        try
        {
            consultarToken();

            if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase) || token.Equals(TokenUser, StringComparison.OrdinalIgnoreCase))
            {

                dt = TraerRegistro("EMP", identificacion);

                Empleado[] objEmpleado = new Empleado[]
                {
                new Empleado()
                {
                    nombres= dt.DefaultView[0].Row["NOMBRE"].ToString(),
                    apellidos= dt.DefaultView[0].Row["APELLIDO"].ToString(),
                    identificacion= dt.DefaultView[0].Row["IDENTIFICACION"].ToString(),
                    direccion= dt.DefaultView[0].Row["DIRECCION"].ToString(),
                    telefono= dt.DefaultView[0].Row["TELEFONO"].ToString(),
                    celular= dt.DefaultView[0].Row["CELULAR"].ToString()
                }
                };
                return new JavaScriptSerializer().Serialize(objEmpleado);
            }
            else
            {
                var jsonData = new
                {
                    mensaje = "token incorrecto"
                };
                return new JavaScriptSerializer().Serialize(jsonData);
            }
        }
        catch(Exception ex)
        {
            var jsonData = new
            {
                mensaje = ex.Message
            };
            return new JavaScriptSerializer().Serialize(jsonData);
        }
    }


    /// <summary>
    /// Consulta el token enviado por la aplicacion, para obtener que corresponda con el
    /// token creado en la base
    /// </summary>
    /// <returns>Si es correcto al realizar la </returns>
    public void consultarToken()
    {
        DataTable dt = null;
        try
        {
            dt = EjecutarSpDataTable("TOKSELECTALL", null);
            dt.DefaultView.RowFilter = "USUARIO = " + "'USER'";
            TokenUser = dt.DefaultView[0].Row["TOKEN"].ToString();
            dt.DefaultView.RowFilter = "USUARIO = " + "'ADMIN'";
            TokenAdmin = dt.DefaultView[0].Row["TOKEN"].ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
        }
    }

    #region Ejecutar SP trae DataTable
            /// <summary>
            /// Ejecuta un SP y retorna un DataTable
            /// </summary>
            /// <param name="nombreSp">Nombre SP</param>
            /// <param name="Parametros">Parametros del SP</param>
            /// <returns>DataTable del SP</returns>
    public DataTable EjecutarSpDataTable(string nombreSp, params System.Object[] Parametros)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = this.ejecutarSPParaDT(nombreSp, Parametros);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return dt;
    }
    #endregion Ejecutar SP trae DataTable

    #region Traer Registro
    public DataTable TraerRegistro(string entidad, params System.Object[] Parametros)
    {
        DataTable dt = null;
        try
        {
            this.NombreEntidad = entidad;
            dt = this.TraerUno(Parametros);
            return dt;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
        }
    }
    #endregion Traer Registro

}