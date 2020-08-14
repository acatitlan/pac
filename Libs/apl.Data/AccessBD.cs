#region Copyright (c) 2000-2013 cjlc
/*
{+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++)
{                                                                   }
{     cjlc Cap control administrativo personal                      }
{                                                                   }
{     Copyrigth (c) 2000-2013 cjlc                                  }
{     Todos los derechos reservados                                 }
{                                                                   }
{*******************************************************************}
 */
/*
 * AccessBD	    Libreria para acceso a Access. Crear base de datos
 *              * Crear base de datos
 *              * Crear tablas, arma la sentencia apropiada y la ejecuta, se necesita la info de la tabla particular
 *              * Si no existe la tabla en el schema la crea.
 *              * Falta si existe la tabla verificar el esquema: crear un nuevo campo
 */
#endregion


using System;
using System.Collections.Generic;

namespace apl.ADOXChunk
{
    using System.Runtime.InteropServices;

    [ComImport, Guid("00000603-0000-0010-8000-00AA006D2EA4")]
    [TypeLibType(TypeLibTypeFlags.FLicensed), InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface CatalogInt
    {
        [DispId(6)]
        [return: MarshalAs(UnmanagedType.Struct)]
        Object Create([In] String connectString);
    }

    [ComImport, Guid("00000602-0000-0010-8000-00AA006D2EA4")]
    public class Catalog
    {
    }
}

namespace apl.Data
{
    #region using
    using System.Data;
    using System.Data.OleDb;
    using System.Collections.Specialized;

    using apl.ADOXChunk;
    #endregion

    /// <summary>
    /// Manejamos las particularidades de Access
    /// </summary>
    public class AccessBD : GenericBD
    {
        #region + override obten coneccion con cadena de coneccion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cadenaConexion"></param>
        /// <returns></returns>
        public override IDbConnection obtenConexion(string cadenaConexion)
        {
            return new OleDbConnection(cadenaConexion);
        }
        #endregion

        #region + override obten parametro
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDataParameter obtenParametro()
        {
            return new OleDbParameter();
        }
        #endregion

        #region + override Get Blob parameter
        protected override IDataParameter GetBlobParameter(IDbConnection connection, IDataParameter p)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        #region # Create data adapter
        protected override IDbDataAdapter CreateDataAdapter()
        {
            return new OleDbDataAdapter();
        }
        #endregion

        #region + Crea tabla
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="campos"></param>
        /// <param name="tipos"></param>
        /// <param name="tamanos"></param>
        /// <param name="keys"></param>
        /// <param name="indexs"></param>
        /// <param name="uniques"></param>
        /// <returns></returns>
        public override bool CreaTabla(string nameTable, StringCollection campos, StringCollection tipos,
            StringCollection tamanos, StringCollection keys, StringCollection indexs, StringCollection uniques)
        {
            bool ok = false;

            if (!ExisteTabla(nameTable))
            {
                string sqlTabla;
                IDataParameter[] parames = null;

                sqlTabla = SentenciaTabla(nameTable, campos, tipos, tamanos, keys, indexs, uniques);
                ejecutaSentencia(sqlTabla, parames);

                if (indexs.Count > 0)
                    foreach(string campo in indexs)
                    {
                        sqlTabla = "CREATE INDEX Indice" + campo;
                        sqlTabla += String.Format(" ON {0} ( {1} )", nameTable, campo);
                        ejecutaSentencia(sqlTabla, parames);
                    }

                ok = true;
            }
            else
            {
                VerificaTabla(nameTable, campos, tipos, tamanos);
            }
            return ok;
        }
        #endregion

        #region - Verifica tabla
        private void VerificaTabla(string nameTable, StringCollection campos, StringCollection tipos, StringCollection tamanos)
        {
            ConnectionState localState = Coneccion.State;
            DataTable schemaTable;


            if (Coneccion.State == ConnectionState.Closed)
                Coneccion.Open();

            schemaTable = (Coneccion as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, nameTable, null });

            for (int i = 0; i < campos.Count; ++i)
            {
                bool exist = false;

                foreach (DataRow row in schemaTable.Rows)
                    if (row["COLUMN_NAME"].ToString() == campos[i])
                    {
                        exist = true;
                        break;
                    }

                if (!exist)
                    AlteraTabla(nameTable, campos[i], tipos[i], tamanos[i]);
            }
            if (localState == ConnectionState.Closed)
                Coneccion.Close();

        }
        #endregion

        #region + override Borra tabla
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameTable"></param>
        public override void BorraTabla(string nameTable)
        {
            if (ExisteTabla(nameTable))
            {
                string sqlTabla;
                IDataParameter[] parames = null;

                sqlTabla = "DROP TABLE ";
                sqlTabla += nameTable;

                ejecutaSentencia(sqlTabla, parames);
            }
        }
        #endregion
        // ------------------------------------------------------------------

        #region - Alter table
        private void AlteraTabla(string nameTabla, string campo, string tipo, string size)
        {
            IDataParameter[] parames = null;
            string sql = "ALTER TABLE ";

            sql += nameTabla;
            sql += " ADD ";
            sql += campo;
            sql += " ";
            sql += tipo;
            if (tipo == "char")
            {
                sql += "(";
                sql += size;
                sql += ")";
            }
            sql = sql.Replace("autoinc", "counter");
            sql = sql.Replace("numeric", "double");

            ejecutaSentencia(sql, parames);


            if (!sql.Contains("counter"))
            {
                sql = " UPDATE ";
                sql += nameTabla;
                sql += " SET ";
                sql += campo;
                sql += " = ";
                
                if (tipo == "char")
                    sql += "' '";
                else if (tipo == "money"
                    || tipo == "numeric")
                    sql += "0.0";
                else if (tipo == "integer")
                    sql += "0";
                else if (tipo == "boolean")
                    sql += "false";
                else if (tipo == "smallint")
                    sql += "0";

                ejecutaSentencia(sql, parames);
            }
        }
        #endregion

        #region # Averigua si existe la tabla en la base de datos
        /// <summary>
        /// la primera vez obtenemos el schema de tablas, para usarlo las siguientes veces
        /// Asi era antes, pero no funciona si borramos y luego creamos una tabla, pues el esquema
        /// ha cambiado, pero en la memoria no ha cambiado.
        /// Era privada pero la hicimos pública para mostrar las tablas que hay.
        /// </summary>
        // private DataTable dtSchema;
        public DataTable SchemaTablas
        {
            get
            {
                DataTable dtSchema;
                ConnectionState localState = Coneccion.State;


                if (Coneccion.State == ConnectionState.Closed)
                    Coneccion.Open();

                dtSchema = (Coneccion as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                if (localState == ConnectionState.Closed)
                    Coneccion.Close();

                return dtSchema;
            }
        }

        public DataTable SchemaTabla(string nameTabla)
        {
            DataTable dtSchema;
            ConnectionState localState = Coneccion.State;


            if (Coneccion.State == ConnectionState.Closed)
                Coneccion.Open();

            dtSchema = (Coneccion as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, nameTabla, null });

            if (localState == ConnectionState.Closed)
                Coneccion.Close();

            return dtSchema;
        }

        /// <summary>
        /// Recorremos la coleccion de tablas del schema, para ver si existe la tabla
        /// con el nombre que nos pasaron
        /// </summary>
        /// <param name="nameTable"></param>
        /// <returns></returns>
        protected bool ExisteTabla(string nameTable)
        {
            bool existe = false;

            foreach (DataRow row in SchemaTablas.Rows)
            {
                existe = row["TABLE_NAME"].ToString() == nameTable;
                if (existe)
                    break;
            }
            return existe;
        }
        #endregion

        #region # Sentencia create table
        /// <summary>
        /// Crea la sentencia para crear la tabla.
        /// </summary>
        /// <param name="nameTable"></param>
        /// <param name="campos"></param>
        /// <param name="tipos"></param>
        /// <param name="tamanos"></param>
        /// <param name="keys"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        protected string SentenciaTabla(string nameTable, StringCollection campos, StringCollection tipos,
            StringCollection tamanos, StringCollection keys, StringCollection indexs, StringCollection uniques)
        {
            string sqlTabla = "";
            string indexSql = "";

            sqlTabla = "CREATE TABLE ";
            sqlTabla += nameTable;
            sqlTabla += " (";
            for (int i = 0; i < campos.Count; ++i)
            {
                sqlTabla += campos[i];

                if (tipos[i] == "char")
                {
                    sqlTabla += " char (";
                    sqlTabla += tamanos[i];
                    sqlTabla += ")";
                }
                else if (tipos[i] == "smallint")
                    sqlTabla += " short";
                else if (tipos[i] == "boolean")
                    sqlTabla += " bit";
                else if (tipos[i] == "numeric")
                    sqlTabla += " double";
                else if (tipos[i] == "money")
                    sqlTabla += " currency";
                else if (tipos[i] == "time")
                    sqlTabla += " datetime";
                else if (tipos[i] == "blob (100, 1)")
                    sqlTabla += " memo";
                else if (tipos[i] == "blob")
                    sqlTabla += " memo";
                else if (tipos[i] == "autoinc")
                    sqlTabla += " UNIQUEIDENTIFIER"; // " counter";
                else
                {
                    sqlTabla += " ";
                    sqlTabla += tipos[i];
                }

                /*
                if (indexs.Contains(campos[i]))
                {
                    sqlTabla += " CONSTRAINT Indice";
                    sqlTabla += campos[i];
                }
                else*/ if (uniques.Contains(campos[i]))
                {
                    sqlTabla += " CONSTRAINT Indice";
                    sqlTabla += campos[i];
                    sqlTabla += " UNIQUE";
                }

                if ((i + 1) < campos.Count)
                    sqlTabla += ", ";
            }
            if (keys.Count > 0)
            {
                indexSql = " CONSTRAINT z PRIMARY KEY (";
                for (int i = 0; i < keys.Count; ++i)
                {
                    indexSql += keys[i];
                    if (i + 1 < keys.Count)
                        indexSql += ", ";
                }
            }
            if (indexSql != string.Empty)
            {
                sqlTabla += ", ";
                sqlTabla += indexSql;
                sqlTabla += ")";
            }

            sqlTabla += ")";


            return sqlTabla;
        }
        #endregion

        #region + override Crea base de datos
        /// <summary>
        /// 
        /// </summary>
        public override void CreaBaseDatos()
        {
            if (!CadenaConeccion.Contains("Paradox"))
            {
                Catalog cat = new Catalog();
                ((CatalogInt)cat).Create(CadenaConeccion);
            }
        }
        #endregion
    }
}
