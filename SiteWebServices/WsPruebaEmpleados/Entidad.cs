using System;
using System.Data;
using System.Data.SqlClient;
using Dyetron.Sicap.Datos;

namespace SiteServices
{
    public class Entidad : System.MarshalByRefObject, IDisposable
    {
        #region Propiedades
        public string NombreEntidad
        {
            get
            {
                return strNombreEntidad;
            }
            set
            {
                strNombreEntidad = value;
            }
        }
        protected System.Data.IDbConnection ConexionBD
        {
            get
            {
                return mServDatos.Conexion;
            }
            set
            {
                mServDatos.Conexion = value;
            }
        }
        public gDatos ServDatos
        {
            get
            {
                return mServDatos;
            }
            set
            {
                mServDatos = value;
            }
        }
        private string _nombreRegistro;


        #endregion

        #region Transacciones
        protected void IniciarTransaccion()
        {
            mServDatos.IniciarTransaccion();
        }
        protected void TerminarTransaccion()
        {
            mServDatos.TerminarTransaccion();
        }
        protected void AbortarTransaccion()
        {
            mServDatos.AbortarTransaccion();
        }
        #endregion

        #region Declaración de Variables
        private string strNombreEntidad = "";
        private Dyetron.Sicap.Datos.gDatos mServDatos = new Dyetron.Sicap.Datos.DatosSQL();

        // Puntero a un recurso externo no administrado.
        private IntPtr handle;
        // Variable que indica si el metodo Dispose ha sido llamado.
        private bool disposed = false;
        #endregion

        #region Propiedades

        #endregion

        #region Constructores y Destructor
        public Entidad()
        {

        }
        public Entidad(string NombreEntidad)
        {
            this.strNombreEntidad = NombreEntidad.ToUpper();
        }

        //Manuel Rojas -- Sobrecarga para conectar con Cronowork
        public Entidad(string NombreEntidad, string nombreRegistro)
        {
            this.strNombreEntidad = NombreEntidad.ToUpper();
            mServDatos = new Dyetron.Sicap.Datos.DatosSQL(nombreRegistro, false);
        }


        ~Entidad()
        {
            Dispose(false);
        }
        #endregion

        #region Lecturas
        /// <summary>
        /// Retorna todos los datos de una tabla
        /// </summary>
        /// <returns></returns>
        public virtual System.Data.DataSet TraerTodos()
        {
            return mServDatos.TraerDataSet(String.Concat(strNombreEntidad, "SELECTALL"));
        }
        /// <summary>
        /// Trae un dataset para llenar un combo
        /// </summary>
        /// <returns></returns>
        public virtual System.Data.DataTable TraerDropDownList()
        {
            return mServDatos.TraerDataTable(String.Concat(this.strNombreEntidad, "LISTA"));
        }
        /// <summary>
        /// Trae un dataset para llenar un combo por algun filtr
        /// </summary>
        /// <param name="Filtro">Nombre del dato por el cual se va a filtrar</param>
        /// <param name="Args">Array de parametros</param>
        /// <returns></returns>
        protected virtual System.Data.DataTable TraerDropDownList(string Filtro, params System.Object[] Args)
        {
            return mServDatos.TraerDataTable(String.Concat(this.strNombreEntidad, "LISTAX", Filtro), Args);
        }

        public virtual System.Data.DataTable TraerUno(params System.Object[] Args)
        {
            return mServDatos.TraerDataTable(String.Concat(this.strNombreEntidad, "SELECTONE"), Args);
        }
        /// <summary>
        /// Trae un dataset segun un criterio de busqueda
        /// </summary>
        /// <param name="Filtro">Nombre del dato por el cual se va a filtrar</param>
        /// <returns></returns>
        protected virtual System.Data.DataSet TraerFiltrado(string Filtro)
        {
            return mServDatos.TraerDataSet(String.Concat(this.strNombreEntidad, "X", Filtro));
        }
        /// <summary>
        /// Trae un dataset segun un criterio de busqueda
        /// </summary>
        /// <param name="Filtro">Nombre del dato por el cual se va a filtrar</param>
        /// <param name="Args">Array de parametros que se envian al procedimiento</param>
        /// <returns></returns>
        protected virtual System.Data.DataSet TraerFiltrado(string Filtro, params System.Object[] Args)
        {
            return mServDatos.TraerDataSet(String.Concat(this.strNombreEntidad, "X", Filtro), Args);
        }
        /// <summary>
        /// Trae un valor que retorne un procedimiento almacenado
        /// </summary>
        /// <param name="Que">Nombre del valor que se va a seleccionar</param>
        /// <param name="Args">Array de parametros que se envian al procedimiento</param>
        /// <returns></returns>
        protected virtual System.Object TraerValor(string Que, params System.Object[] Args)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "SELECTVALUE", Que), Args);
        }



        /// <summary>
        /// Ejecuta el sp indicado y retorna el correspondiente DataSet
        /// </summary>
        /// <param name="SP"></param>
        /// <returns></returns>
        protected virtual System.Data.DataSet ejecutarSP(string SP)
        {
            return mServDatos.TraerDataSet(SP);
        }

        protected virtual System.Data.DataSet ejecutarSPParaDS(string SP, params System.Object[] Args)
        {
            return mServDatos.TraerDataSet(SP, Args);
        }

        protected virtual System.Data.DataSet ejecutarSPConObjeto(string SP, object objExamine)
        {
            return mServDatos.TraerDataSet(SP, objExamine);
        }

        protected virtual System.Data.DataTable ejecutarSPParaDT(string SP, params System.Object[] Args)
        {
            return mServDatos.TraerDataTable(SP, Args);
        }

        public virtual System.Data.DataTable PublicEjecutarSPParaDT(string SP, params System.Object[] Args)
        {
            return mServDatos.TraerDataTable(SP, Args);
        }

        protected object ejecutarSP(string SP, params System.Object[] Args)
        {
            return mServDatos.Ejecutar(SP, Args);
        }

        protected object ejecutarSP(string SP, System.Data.DataRow ObjRow)
        {
            return mServDatos.Ejecutar(SP, ObjRow);
        }


        #endregion

        #region Acciones

        protected object Agregar(params System.Object[] Args)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "INSERT"), Args);
        }
        protected object Agregar(System.Data.DataRow ObjRow)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "INSERT"), ObjRow);
        }
        protected object Agregar(Object ObjExamine)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "INSERT"), ObjExamine);
        }
        protected void Agregar(Object ObjExamine, System.Data.DataTable Dt)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "INSERT"), ObjExamine, Dt);
        }

        protected object Actualizar(params System.Object[] Args)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "UPDATE"), Args);
        }
        protected object Actualizar(System.Data.DataRow ObjRow)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "UPDATE"), ObjRow);
        }
        protected object Actualizar(Object ObjExamine)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "UPDATE"), ObjExamine);
        }
        protected void Actualizar(Object ObjExamine, System.Data.DataTable Dt)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "UPDATE"), ObjExamine, Dt);
        }


        protected void Borrar(params System.Object[] Args)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "DELETE"), Args);
        }
        protected void Borrar(System.Data.DataRow ObjRow)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "DELETE"), ObjRow);
        }
        protected void Borrar(Object ObjExamine)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "DELETE"), ObjExamine);
        }
        protected void Borrar(Object ObjExamine, System.Data.DataTable Dt)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, "DELETE"), ObjExamine, Dt);
        }

        // here
        protected object Ejecutar(string Nombre, params System.Object[] Args)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, Nombre), Args);
        }
        protected object EjecutarSP(string NombreSP, params System.Object[] Args)
        {
            return mServDatos.Ejecutar(NombreSP, Args);
        }
        protected void Ejecutar(string Nombre, System.Data.DataRow ObjRow)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, Nombre), ObjRow);
        }
        protected object Ejecutar(string Nombre, Object ObjExamine)
        {
            return mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, Nombre), ObjExamine);
        }
        protected void Ejecutar(string Nombre, Object ObjExamine, System.Data.DataTable Dt)
        {
            mServDatos.Ejecutar(String.Concat(this.strNombreEntidad, Nombre), ObjExamine, Dt);
        }


        protected void EjecutarSPRemoto(string textoQuery)
        {
            mServDatos.EjecutarRemoto(textoQuery);
        }
        #endregion

        #region Remoting
        public override System.Runtime.Remoting.ObjRef CreateObjRef(System.Type requestedType)
        {
            return new System.Runtime.Remoting.ObjRef(this, requestedType);
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Este método se encarga de limpiar y destruir el objeto entidad
        /// </summary>
        // Elaborado Por            : Luis Roberto García Paipillla
        // Fecha Creación           : 2004/12/08
        // Modificado por           : 
        // Fecha Modificación       : 
        // Descripción Modificacion : 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Este método se encarga de limpiar y destruir el objeto entidad
        /// </summary>
        /// <param name="disposing">Variable Booleana que indica si es TRUE que el metodo fue llamado directa o indirectamente por el usuario
        ///, en cambio, si es FALSE es porque fue llamado por el garbage collector</param>
        /// </summary>
        // Elaborado Por            : Luis Roberto García Paipillla
        // Fecha Creación           : 2004/12/08
        // Modificado por           : 
        // Fecha Modificación       : 
        // Descripción Modificacion : 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Libera los recursos administrados
                    mServDatos.Dispose();
                }
                // Libera los recursos  no administrados            
                // solo si este codigo es ejecutado.
                CloseHandle(handle);
                handle = IntPtr.Zero;

            }
            disposed = true;
        }

        //Este método sirve para limpiar los recursos no administrados
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        #endregion
    }
}