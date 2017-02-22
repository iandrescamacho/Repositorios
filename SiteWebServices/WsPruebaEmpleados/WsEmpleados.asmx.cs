using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;

namespace WsPruebaEmpleados
{
    /// <summary>
    /// Descripción breve de WsEmpleados
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WsEmpleados : System.Web.Services.WebService
    {
        #region Atributos
        private string TokenUser { get; set; }
        private string TokenAdmin { get; set; }

        #endregion Atributos

        #region Metodos

        #region Metodo Consulta

        #region XML
        /// <summary>
        /// Metodo que consulta un empleado por su identificacion
        /// </summary>
        /// <param name="identificacion">numero de identificacion del empleado</param>
        /// <returns>datos XML</returns>
        [WebMethod]
        public Empleado[] getEmpleadoXML(string token, string identificacion)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase) || token.Equals(TokenUser, StringComparison.OrdinalIgnoreCase))
                {
                    dt = objService.TraerRegistro("EMP", identificacion);

                    if (dt.Rows.Count > 0)
                    {
                        Empleado[] objEmpleado = new Empleado[]
                        {
                            new Empleado()
                            {
                                nombres= dt.DefaultView[0].Row["NOMBRE"].ToString(),
                                apellidos= dt.DefaultView[0].Row["APELLIDO"].ToString(),
                                identificacion= dt.DefaultView[0].Row["IDENTIFICACION"].ToString(),
                                direccion= dt.DefaultView[0].Row["DIRECCION"].ToString(),
                                telefono= dt.DefaultView[0].Row["TELEFONO"].ToString(),
                                celular= dt.DefaultView[0].Row["CELULAR"].ToString(),
                                email= dt.DefaultView[0].Row["EMAIL"].ToString()
                            }
                        };
                        return objEmpleado;
                    }
                    else
                    {
                        Empleado[] objEmpleado = new Empleado[]
                        {
                            new Empleado()
                            {
                                Mensaje = "NO EXISTE UN EMPLEADO CON IDENTIFICACION: " + identificacion
                            }
                        };
                        return objEmpleado;
                    }
                }
                else
                {
                    Empleado[] objEmpleado = new Empleado[]
                    {
                        new Empleado()
                        {
                            Mensaje = "Token no valido"
                        }
                    };
                    return objEmpleado;
                }
            }
            catch (Exception ex)
            {
                Empleado[] objEmpleado = new Empleado[]
                {
                    new Empleado()
                    {
                        Mensaje = ex.Message
                    }
                };
                return objEmpleado;
            }
        }
        #endregion XML

        #region JSON
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getEmpleadoJSON(string token, string identificacion)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase) || token.Equals(TokenUser, StringComparison.OrdinalIgnoreCase))
                {

                    dt = objService.TraerRegistro("EMP", identificacion);

                    if (dt.Rows.Count > 0)
                    {
                        Empleado[] objEmpleado = new Empleado[]
                        {
                            new Empleado()
                            {
                                nombres= dt.DefaultView[0].Row["NOMBRE"].ToString(),
                                apellidos= dt.DefaultView[0].Row["APELLIDO"].ToString(),
                                identificacion= dt.DefaultView[0].Row["IDENTIFICACION"].ToString(),
                                direccion= dt.DefaultView[0].Row["DIRECCION"].ToString(),
                                telefono= dt.DefaultView[0].Row["TELEFONO"].ToString(),
                                celular= dt.DefaultView[0].Row["CELULAR"].ToString(),
                                email= dt.DefaultView[0].Row["EMAIL"].ToString()
                            }
                        };
                        return new JavaScriptSerializer().Serialize(objEmpleado);
                    }
                    else
                    {
                        var jsonData = new
                        {
                            Mensaje = "NO EXISTE UN EMPLEADO CON IDENTIFICACION: " + identificacion
                        };
                        return new JavaScriptSerializer().Serialize(jsonData);
                    }
                }
                else
                {
                    var jsonData = new
                    {
                        mensaje = "Token no valido"
                    };
                    return new JavaScriptSerializer().Serialize(jsonData);
                }
            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    mensaje = ex.Message
                };
                return new JavaScriptSerializer().Serialize(jsonData);
            }
        }

        #endregion JSON

        #endregion Metodo Consulta

        #region Metodo Insertar

        #region XML
        /// <summary>
        /// Metodo que consulta un empleado por su identificacion
        /// </summary>
        /// <param name="identificacion">numero de identificacion del empleado</param>
        /// <returns>datos XML</returns>
        [WebMethod]
        public Token[] insertEmpleadoXML(string token, string nombres, string apellidos, string identificacion, string direccion, string telefono, string celular, string email)
        {
            ServiceClass objService = new ServiceClass();
            string mensaje = "";
            int estadoTransaccion = 0;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    estadoTransaccion = objService.ejecutarSPTraeInt("EMPINSERT", nombres, apellidos, identificacion, direccion, telefono, celular, email);

                    switch (estadoTransaccion)
                    {
                        case 0:
                            mensaje = "YA EXISTE UN EMPLEADO CON LA IDENTIFICACION: " + identificacion;
                            break;
                        case 1:
                            mensaje = "SE CREO EL EMPLEADO EXITOSAMENTE";
                            break;
                    }
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = mensaje
                        }
                    };
                    return objMensajes;
                }
                else
                {
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = "Token no valido"
                        }
                    };
                    return objMensajes;
                }
            }
            catch (Exception ex)
            {
                Token[] objMensajes = new Token[]
                {
                    new Token()
                    {
                        Mensaje = ex.Message
                    }
                };
                return objMensajes;
            }
        }
        #endregion XML

        #region JSON
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string insertEmpleadoJSON(string token, string nombres, string apellidos, string identificacion, string direccion, string telefono, string celular, string email)
        {
            ServiceClass objService = new ServiceClass();
            string mensaje = "";
            int estadoTransaccion = 0;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    estadoTransaccion = objService.ejecutarSPTraeInt("EMPINSERT", nombres, apellidos, identificacion, direccion, telefono, celular, email);

                    switch (estadoTransaccion)
                    {
                        case 0:
                            mensaje = "YA EXISTE UN EMPLEADO CON LA IDENTIFICACION: " + identificacion;
                            break;
                        case 1:
                            mensaje = "SE CREO EL EMPLEADO EXITOSAMENTE";
                            break;
                    }
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = mensaje
                        }
                    };
                    return new JavaScriptSerializer().Serialize(objMensajes);
                }
                else
                {
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = "Token no valido"
                        }
                    };
                    return new JavaScriptSerializer().Serialize(objMensajes);
                }
            }
            catch (Exception ex)
            {
                Token[] objMensajes = new Token[]
                {
                    new Token()
                    {
                        Mensaje = ex.Message
                    }
                };
                return new JavaScriptSerializer().Serialize(objMensajes);
            }
        }

        #endregion JSON

        #endregion Metodo Insertar

        #region Metodo Actualizar

        #region XML
        /// <summary>
        /// Metodo que consulta un empleado por su identificacion
        /// </summary>
        /// <param name="identificacion">numero de identificacion del empleado</param>
        /// <returns>datos XML</returns>
        [WebMethod]
        public Token[] updateEmpleadoXML(string token, string nombres, string apellidos, string identificacion, string direccion, string telefono, string celular, string email)
        {
            ServiceClass objService = new ServiceClass();
            string mensaje = "";
            int estadoTransaccion = 0;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    estadoTransaccion = objService.ejecutarSPTraeInt("EMPUDPATE", nombres, apellidos, identificacion, direccion, telefono, celular, email);

                    switch (estadoTransaccion)
                    {
                        case 0:
                            mensaje = "NO EXISTE UN EMPLEADO CON LA IDENTIFICACION: " + identificacion;
                            break;
                        case 1:
                            mensaje = "SE CREO EL EMPLEADO EXITOSAMENTE";
                            break;
                    }
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = mensaje
                        }
                    };
                    return objMensajes;
                }
                else
                {
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = "Token no valido"
                        }
                    };
                    return objMensajes;
                }
            }
            catch (Exception ex)
            {
                Token[] objMensajes = new Token[]
                {
                    new Token()
                    {
                        Mensaje = ex.Message
                    }
                };
                return objMensajes;
            }
        }
        #endregion XML

        #region JSON
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string updateEmpleadoJSON(string token, string nombres, string apellidos, string identificacion, string direccion, string telefono, string celular, string email)
        {
            ServiceClass objService = new ServiceClass();
            string mensaje = "";
            int estadoTransaccion = 0;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase))
                {
                    estadoTransaccion = objService.ejecutarSPTraeInt("EMPUPDATE", nombres, apellidos, identificacion, direccion, telefono, celular, email);

                    switch (estadoTransaccion)
                    {
                        case 0:
                            mensaje = "NO EXISTE UN EMPLEADO CON LA IDENTIFICACION: " + identificacion;
                            break;
                        case 1:
                            mensaje = "SE ACTUALIZO EL EMPLEADO EXITOSAMENTE";
                            break;
                    }
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = mensaje
                        }
                    };
                    return new JavaScriptSerializer().Serialize(objMensajes);
                }
                else
                {
                    Token[] objMensajes = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = "Token no valido"
                        }
                    };
                    return new JavaScriptSerializer().Serialize(objMensajes);
                }
            }
            catch (Exception ex)
            {
                Token[] objMensajes = new Token[]
                {
                    new Token()
                    {
                        Mensaje = ex.Message
                    }
                };
                return new JavaScriptSerializer().Serialize(objMensajes);
            }
        }

        #endregion JSON

        #endregion Metodo Actualizar

        #region Metodo Consultar Token
        /// <summary>
        /// Consulta el token enviado por la aplicacion, para obtener que corresponda con el
        /// token creado en la base
        /// </summary>
        /// <returns>Si es correcto al realizar la </returns>
        public void consultarToken()
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                dt = objService.EjecutarSpDataTable("TOKSELECTALL", null);
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

        #endregion Metodo Consultar Token

        #region Metodo Consultar Todos Empleados

        #region XML
        /// <summary>
        /// Metodo que consulta un empleado por su identificacion
        /// </summary>
        /// <param name="identificacion">numero de identificacion del empleado</param>
        /// <returns>datos XML</returns>
        [WebMethod]
        public DataTable getAllEmpleadoXML(string token)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = new DataTable("PRUEBA");
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase) || token.Equals(TokenUser, StringComparison.OrdinalIgnoreCase))
                {
                    dt = objService.EjecutarSpDataTable("EMPSELECTALL");
                    dt.TableName = "PRUEBA";
                    return dt;
                }
                else
                {
                    dt.Columns.Add("Mensaje", typeof(string));
                    dt.Rows.Add("Token no valido");
                    return dt;
                }
            }
            catch (Exception ex)
            {
                dt.Columns.Add("Mensaje", typeof(string));
                dt.Rows.Add(ex.Message);
                return dt;
            }
        }
        #endregion XML

        #region JSON
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getAllEmpleadoJSON(string token)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                consultarToken();

                if (token.Equals(TokenAdmin, StringComparison.OrdinalIgnoreCase) || token.Equals(TokenUser, StringComparison.OrdinalIgnoreCase))
                {
                    dt = objService.EjecutarSpDataTable("EMPSELECTALL");

                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    Dictionary<string, object> row;
                    foreach (DataRow dr in dt.Rows)
                    {
                        row = new Dictionary<string, object>();
                        foreach (DataColumn col in dt.Columns)
                        {
                            row.Add(col.ColumnName, dr[col]);
                        }
                        rows.Add(row);
                    }
                    return serializer.Serialize(rows);

                }
                else
                {
                    var jsonData = new
                    {
                        mensaje = "Token no valido"
                    };
                    return new JavaScriptSerializer().Serialize(jsonData);
                }
            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    mensaje = ex.Message
                };
                return new JavaScriptSerializer().Serialize(jsonData);
            }
        }

        #endregion JSON

        #endregion Metodo Consultar Todos Empleados

        #region Consultar Token

        #region XML
        /// <summary>
        /// Metodo que consulta un empleado por su identificacion
        /// </summary>
        /// <param name="identificacion">numero de identificacion del empleado</param>
        /// <returns>datos XML</returns>
        [WebMethod]
        public Token[] getTokenXML(string user, string password)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                dt = objService.EjecutarSpDataTable("TOKSELECTONE", user, Encriptar(password));

                if (dt.Rows.Count > 0)
                {
                    Token[] objToken = new Token[]
                    {
                        new Token()
                        {
                            token = dt.DefaultView[0].Row["TOKEN"].ToString()
                        }
                    };
                    return objToken;
                }
                else
                {
                    Token[] objToken = new Token[]
                    {
                        new Token()
                        {
                            Mensaje = "Usuario o password no validos"
                        }
                    };
                    return objToken;
                }
            }
            catch (Exception ex)
            {
                Token[] objToken = new Token[]
                {
                    new Token()
                    {
                        Mensaje = ex.Message
                    }
                };
                return objToken;
            }
        }
        #endregion XML

        #region JSON
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string getTokenJSON(string user, string password)
        {
            ServiceClass objService = new ServiceClass();
            DataTable dt = null;
            try
            {
                dt = objService.EjecutarSpDataTable("TOKSELECTONE", user, Encriptar(password));

                if (dt.Rows.Count > 0)
                {
                    Token[] objToken = new Token[]
                    {
                        new Token()
                        {
                            token = dt.DefaultView[0].Row["TOKEN"].ToString()
                        }
                    };
                    return new JavaScriptSerializer().Serialize(objToken);
                }
                else
                {
                    var jsonData = new
                    {
                        Mensaje = "Usuario o password no validos"
                    };
                    return new JavaScriptSerializer().Serialize(jsonData);
                }
            }
            catch (Exception ex)
            {
                var jsonData = new
                {
                    mensaje = ex.Message
                };
                return new JavaScriptSerializer().Serialize(jsonData);
            }
        }

        #endregion JSON

        #endregion Consultar Token

        #region Encriptar
        private string Encriptar(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        #endregion Encriptar

        #region DesEncriptar
        private string DesEncriptar(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        #endregion DesEncriptar

        #endregion Metodos
    }
}
