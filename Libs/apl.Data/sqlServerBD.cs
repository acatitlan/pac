/*
 * User: Tlacaelel Icpac
 * Date: Mié Jun 30 2004
 * Time: 03:51 p.m.
 * 
 * Derechos:	(C) 2004-2004 by icpac
 * Correo:		tlacaelel.icpac@gmail.com
 * 
 * sqlServerD.cs  -  Acceso a la base de datos Sql Server
 */

using System;
using System.Data;
using System.Data.SqlClient;

using System.Collections;
using System.Data.OleDb;
using System.Collections.Specialized;

namespace apl.Data
{
	//TODO: Cómo hacer esto más general?
	//
	public class DBTypeConversion
	{
		private static String[,] DBTypeConversionKey = new String[,]
		{
			/*
			{"BigInt","System.Int64"},
			{"Binary","System.Byte[]"},
			{"Bit","System.Boolean"},
			{"Char","System.String"},
			{"DateTime","System.DateTime"},
			{"Decimal","System.Decimal"},
			{"Float","System.Double"},
			{"Image","System.Byte[]"},
			{"Int","System.Int32"},
			{"Money","System.Decimal"},
			{"NChar","System.String"},
			{"NText","System.String"},
			{"NVarChar","System.String"},
			{"Real","System.Single"},
			{"SmallDateTime","System.DateTime"},
			{"SmallInt","System.Int16"},
			{"SmallMoney","System.Decimal"},
			{"Text","System.String"},
			{"Timestamp","System.DateTime"},
			{"TinyInt","System.Byte"},
			{"UniqueIdentifer","System.Guid"},
			{"VarBinary","System.Byte[]"},
			{"VarChar","System.String"},
			{"Variant","System.Object"}*/
			{"AnsiString","System.Object"},
			{"AnsiStringFixedLength ","System.Object"},
			{"Binary","System.Object"},
			{"Boolean","System.Boolean"}, 
			{"Byte","System.Byte"}, 
			{"Currency","System.Object"},
			{"Date","System.Object"},
			{"DateTime","System.DateTime"}, 
			{"Decimal","System.Decimal"},
			{"Double","System.Object"},
			{"Guid","System.Object"},
			{"Int16","System.Object"},
			{"Int32","System.Int32"},
			{"Int64","System.Object"},
			{"Object","System.Object"},
			{"SByte","System.Object"},
			{"Single","System.Object"},
			{"String","System.String"},
			{"StringFixedLength","System.Object"},
			{"Time","System.Object"},
			{"UInt16","System.Object"},
			{"UInt32","System.Object"},
			{"UInt64","System.Object"},
			{"VarNumeric","System.Object"}
		};


		public static /*Sql*/DbType SystemTypeToDbType( System.Type sourceType )
		{
			/*Sql*/DbType	result;
			String		SystemType	= sourceType.ToString();
			String		DBType		= String.Empty;
			int			keyCount	= DBTypeConversionKey.GetLength(0);

			for(int i=0;i<keyCount;i++)
			{
				if(DBTypeConversionKey[i,1].Equals(SystemType)) 
					DBType = DBTypeConversionKey[i,0];
			}

			if (DBType==String.Empty) DBType = "Variant";
	        
			result = (/*Sql*/DbType)Enum.Parse(typeof(/*Sql*/DbType), DBType);
			return result;
		}

		public static Type DbTypeToSystemType( SqlDbType sourceType )
		{
			Type	result;
			String	SystemType	= String.Empty;
			String	DBType		= sourceType.ToString();
			int		keyCount	= DBTypeConversionKey.GetLength(0);

			for(int i=0;i<keyCount;i++)
			{
				if(DBTypeConversionKey[i,0].Equals(DBType)) 
					SystemType = DBTypeConversionKey[i,1];
			}

			if (SystemType==String.Empty) SystemType = "System.Object";

			result = Type.GetType(SystemType);
			return result;
		}
	} 


	#region sqlServerBD
	/// <summary>
	/// Description of sqlServerBD.	
	/// </summary>
	public class sqlServerBD : GenericBD
	{
		#region Constructores
		public sqlServerBD()
		{
		}
		#endregion

		#region Override 
		#region Conexion
		/*
		public override IDbConnection obtenConexion(string conexion)
		{
			return new SqlConnection();
		}*/
		public override IDbConnection obtenConexion(string conexion)
		{
			return new SqlConnection(conexion);
		}
	    #endregion

		#region Adapter
		protected override IDbDataAdapter CreateDataAdapter()
		{
			return new SqlDataAdapter();
		}
		#endregion

		#region Parameter
		protected override IDataParameter GetBlobParameter(IDbConnection connection, IDataParameter p)
		{
			// do nothing special for BLOBs...as far as we know now.
			return p;
		}
		public override IDataParameter obtenParametro()
		{
			return new SqlParameter(); 
		}

		/// <summary>
		/// Calls the CommandBuilder.DeriveParameters method for the specified provider, doing any setup and cleanup necessary
		/// </summary>
		/// <param name="cmd">The IDbCommand referencing the stored procedure from which the parameter information is to be derived. The derived parameters are added to the Parameters collection of the IDbCommand. </param>
		public /*override*/ void DeriveParameters( IDbCommand cmd )
		{
			bool mustCloseConnection = false;

			if( !( cmd is SqlCommand ) )
				throw new ArgumentException( "The command provided is not a SqlCommand instance.", "cmd" );
			
			if (cmd.Connection.State != ConnectionState.Open) 
			{
				cmd.Connection.Open();
				mustCloseConnection = true;
			}
			
			SqlDeriveParameters.DeriveParameters((SqlCommand)cmd );
			
			if (mustCloseConnection)
			{
				cmd.Connection.Close();
			}
		}
		#endregion

		#region Command
		public /*override*/ IDbCommand obtenCommand()
		{
			return new SqlCommand();
		}
		#endregion

		#region Type to DbType
		public /*override*/ DbType TypeToDbType(Type sourceType)
		{
			return DBTypeConversion.SystemTypeToDbType(sourceType);
			// return DbType.String;
		}
		#endregion

		/*
		public override DataSet openSQL(string sentencia)
		{
			try
			{
				if (sentencia == null || sentencia.Length <= 0) throw new ArgumentNullException("'sqlServerBD.cs'; 'sentencia' en 'openSQL'");

				string			strConn		= "server=LOCALHOST;database=icpac;user id=OpusAdmin;password=OpusAdmin";
				SqlConnection	connection	= new SqlConnection();
				DataSet			dataset		= new DataSet();
				SqlDataAdapter  adapter		= new SqlDataAdapter();

				connection.ConnectionString = strConn;
				connection.Open();

				adapter.SelectCommand = new SqlCommand(sentencia, connection);
				adapter.Fill(dataset);
				connection.Close();

				return dataset;
			}
			catch (InvalidOperationException e)
			{
				throw new InvalidOperationException("Conexión abierta o no es correcta la cadena de conexión.", e);
			}
			catch (SqlException e)
			{
				throw new InvalidOperationException("Error desconocido en Open.", e);
			}
		}*/

		/*
		public override bool execSQL(string sentencia)
		{
			try
			{
				if (sentencia == null || sentencia.Length <= 0) throw new ArgumentNullException("'sqlServerBD.cs'; 'sentencia' en 'openSQL'");

				string			strConn		= "server=LOCALHOST;database=icpac;user id=OpusAdmin;password=OpusAdmin";
				SqlConnection	connection	= new SqlConnection();

				connection.ConnectionString = strConn;
				connection.Open();

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sentencia;
					command.ExecuteNonQuery();
				}

				connection.Close();
				return true;
			}
			catch (InvalidOperationException e)
			{
				throw new InvalidOperationException("Conexión abierta o no es correcta la cadena de conexión.", e);
			}
			catch (SqlException e)
			{
				throw new InvalidOperationException("Error desconocido en Open.", e);
			}
		}*/
		#endregion

		public override bool CreaTabla(string name, StringCollection cmps, StringCollection tips, StringCollection tams, StringCollection keys, StringCollection indexs, StringCollection uniques)
		{
			throw new NotImplementedException();
		}

		public override void BorraTabla(string name)
		{
			throw new NotImplementedException();
		}
	}
	#endregion
}

#region Derive Parameters
// We create our own class to do this because the existing ADO.NET 1.1 implementation is broken.
internal class SqlDeriveParameters 
{
 
	internal static void DeriveParameters(SqlCommand cmd) 
	{

		string  cmdText;
		SqlCommand newCommand;
		SqlDataReader reader;
		ArrayList parameterList;
		SqlParameter sqlParam;
		CommandType cmdType;
		string  procedureSchema;
		string  procedureName;
		int groupNumber;
		SqlTransaction trnSql = cmd.Transaction;

		cmdType = cmd.CommandType;

		if ((cmdType == CommandType.Text) ) 
		{
			throw new InvalidOperationException();
		} 
		else if ((cmdType == CommandType.TableDirect) ) 
		{
			throw new InvalidOperationException();
		} 
		else if ((cmdType != CommandType.StoredProcedure) ) 
		{
			throw new InvalidOperationException();
		}

		procedureName = cmd.CommandText;
		string server = null;
		string database = null;
		procedureSchema = null;

		// split out the procedure name to get the server, database, etc.
		GetProcedureTokens(ref procedureName, ref server, ref database, ref procedureSchema);

		// look for group numbers
		groupNumber = ParseGroupNumber(ref procedureName);

		newCommand = null;

		// set up the command string.  We use sp_procuedure_params_rowset to get the parameters
		if (database != null) 
		{
			cmdText = string.Concat("[", database, "]..sp_procedure_params_rowset");
			if (server != null ) 
			{
				cmdText = string.Concat(server, ".", cmdText);
			}

			// be careful of transactions
			if (trnSql != null ) 
			{
				newCommand = new SqlCommand(cmdText, cmd.Connection, trnSql);
			} 
			else 
			{
				newCommand = new SqlCommand(cmdText, cmd.Connection);
			}
		} 
		else 
		{
			// be careful of transactions
			if (trnSql != null ) 
			{
				newCommand = new SqlCommand("sp_procedure_params_rowset", cmd.Connection, trnSql);
			} 
			else 
			{
				newCommand = new SqlCommand("sp_procedure_params_rowset", cmd.Connection);
			}
		}

		newCommand.CommandType = CommandType.StoredProcedure;
		newCommand.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 255));
		newCommand.Parameters[0].Value = procedureName;

		// make sure we specify 
		if (! IsEmptyString(procedureSchema) ) 
		{
			newCommand.Parameters.Add(new SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255));
			newCommand.Parameters[1].Value = procedureSchema;
		}

		// make sure we specify the groupNumber if we were given one
		if ( groupNumber != 0 ) 
		{
			newCommand.Parameters.Add(new SqlParameter("@group_number", groupNumber));
		}

		reader = null;
		parameterList = new ArrayList();

		try 
		{
			// get a reader full of our params
			reader = newCommand.ExecuteReader();
			sqlParam = null;

			while ( reader.Read()) 
			{
				// get all the parameter properties that we can get, Name, type, length, direction, precision
				sqlParam = new SqlParameter();
				sqlParam.ParameterName = (string)(reader["PARAMETER_NAME"]);
				sqlParam.SqlDbType = GetSqlDbType((short)(reader["DATA_TYPE"]), (string)(reader["TYPE_NAME"]));

				if (reader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ) 
				{
					sqlParam.Size = (int)(reader["CHARACTER_MAXIMUM_LENGTH"]);
				}

				sqlParam.Direction = GetParameterDirection((short)(reader["PARAMETER_TYPE"]));

				if ((sqlParam.SqlDbType == SqlDbType.Decimal) ) 
				{
					sqlParam.Scale = (byte)(((short)(reader["NUMERIC_SCALE"]) & 255));
					sqlParam.Precision = (byte)(((short)(reader["NUMERIC_PRECISION"]) & 255));
				}
				parameterList.Add(sqlParam);
			}
		} 
		finally 
		{
			// close our reader and connection when we're done
			if (reader != null) 
			{
				reader.Close();
			}
			newCommand.Connection = null;
		}

		// we didn't get any parameters
		if ((parameterList.Count == 0) ) 
		{
			throw new InvalidOperationException();
		}

		cmd.Parameters.Clear();

		// add the parameters to the command object
		foreach ( object parameter in parameterList ) 
		{
			cmd.Parameters.Add(parameter);
		} 


	}

	/// <summary>
	/// Checks to see if the stored procedure being called is part of a group, then gets the group number if necessary
	/// </summary>
	/// <param name="procedure">Stored procedure being called.  This method may change this parameter by removing the group number if it exists.</param>
	/// <returns>the group number</returns>
	private static int ParseGroupNumber(ref string procedure) 
	{
		string  newProcName;
		int groupPos = procedure.IndexOf(';');
		int groupIndex = 0;

		if ( groupPos > 0 ) 
		{
			newProcName = procedure.Substring(0, groupPos);
			try 
			{
				groupIndex = int.Parse(procedure.Substring(groupPos + 1));
			} 
			catch  
			{
				throw new InvalidOperationException();
			}
		} 
		else 
		{
			newProcName = procedure;
			groupIndex = 0;
		}

		procedure = newProcName;
		return groupIndex;
	}

	/// <summary>
	/// Tokenize the procedure string
	/// </summary>
	/// <param name="procedure">The procedure name</param>
	/// <param name="server">The server name</param>
	/// <param name="database">The database name</param>
	/// <param name="owner">The owner name</param>
	private static void GetProcedureTokens( ref string  procedure, ref string server, ref string database, ref string owner) 
	{

		string [] spNameTokens;
		int arrIndex;
		int nextPos;
		int currPos;
		int tokenCount;

		server = database = owner = null;

		spNameTokens = new string [4];

		if ( ! IsEmptyString(procedure) ) 
		{
			
			arrIndex = 0;
			nextPos = 0;
			currPos = 0;

			while ((arrIndex < 4)) 
			{
				currPos = procedure.IndexOf('.', nextPos);
				if ((-1 == currPos) ) 
				{
					spNameTokens[arrIndex] = procedure.Substring(nextPos);
					break;
				}
				spNameTokens[arrIndex] = procedure.Substring(nextPos, (currPos - nextPos));
				nextPos = (currPos + 1);
				if ((procedure.Length <= nextPos) ) 
				{
					break;
				}
				arrIndex = (arrIndex + 1);
			}

			tokenCount = arrIndex + 1;

			// based on how many '.' we found, we know what tokens we found
			switch (tokenCount) 
			{
				case 1:
					procedure = spNameTokens[0];
					break;

				case 2:
					procedure = spNameTokens[1];
					owner = spNameTokens[0];
					break;

				case 3:
					procedure = spNameTokens[2];
					owner = spNameTokens[1];
					database = spNameTokens[0];
					break;

				case 4:
					procedure = spNameTokens[3];
					owner = spNameTokens[2];
					database = spNameTokens[1];
					server = spNameTokens[0];
					break;
			}
		}
	}

	/// <summary>
	/// Checks for an empty string
	/// </summary>
	/// <param name="str">String to check</param>
	/// <returns>boolean value indicating whether string is empty</returns>
	private static bool IsEmptyString( string  str) 
	{
		if (str != null ) 
		{
			return (0 == str.Length);
		}
		return true;
	}

	/// <summary>
	/// Convert OleDbType to SQlDbType
	/// </summary>
	/// <param name="paramType">The OleDbType to convert</param>
	/// <param name="typeName">The typeName to convert for items such as Money and SmallMoney which both map to OleDbType.Currency</param>
	/// <returns>The converted SqlDbType</returns>
	private static SqlDbType GetSqlDbType( short paramType,  string  typeName) 
	{
		SqlDbType cmdType;
		OleDbType oleDbType;
		cmdType = SqlDbType.Variant;
		oleDbType = (OleDbType)(paramType);

		switch (oleDbType) 
		{
			case OleDbType.SmallInt:
				cmdType = SqlDbType.SmallInt;
				break;
			case OleDbType.Integer:
				cmdType = SqlDbType.Int;
				break;
			case OleDbType.Single:
				cmdType = SqlDbType.Real;
				break;
			case OleDbType.Double:
				cmdType = SqlDbType.Float;
				break;
			case OleDbType.Currency:
				cmdType = (typeName == "money") ?  SqlDbType.Money : SqlDbType.SmallMoney;
				break;
			case OleDbType.Date:
				cmdType = (typeName == "datetime") ? SqlDbType.DateTime :  SqlDbType.SmallDateTime;
				break;
			case OleDbType.BSTR:
				cmdType = (typeName == "nchar") ? SqlDbType.NChar : SqlDbType.NVarChar;
				break;
			case OleDbType.Boolean:
				cmdType = SqlDbType.Bit;
				break;
			case OleDbType.Variant:
				cmdType = SqlDbType.Variant;
				break;
			case OleDbType.Decimal:
				cmdType = SqlDbType.Decimal;
				break;
			case OleDbType.TinyInt:
				cmdType = SqlDbType.TinyInt;
				break;
			case OleDbType.UnsignedTinyInt:
				cmdType = SqlDbType.TinyInt;
				break;
			case OleDbType.UnsignedSmallInt:
				cmdType = SqlDbType.SmallInt;
				break;
			case OleDbType.BigInt:
				cmdType = SqlDbType.BigInt;
				break;
			case OleDbType.Filetime:
				cmdType = (typeName == "datetime") ? SqlDbType.DateTime : SqlDbType.SmallDateTime;
				break;
			case OleDbType.Guid:
				cmdType = SqlDbType.UniqueIdentifier;
				break;
			case OleDbType.Binary:
				cmdType = (typeName == "binary") ? SqlDbType.Binary : SqlDbType.VarBinary;
				break;
			case OleDbType.Char:
				cmdType = (typeName == "char") ? SqlDbType.Char : SqlDbType.VarChar;
				break;
			case OleDbType.WChar:
				cmdType = (typeName == "nchar") ? SqlDbType.NChar : SqlDbType.NVarChar;
				break;
			case OleDbType.Numeric:
				cmdType = SqlDbType.Decimal;
				break;
			case OleDbType.DBDate:
				cmdType = (typeName == "datetime") ? SqlDbType.DateTime : SqlDbType.SmallDateTime;
				break;
			case OleDbType.DBTime:
				cmdType = (typeName == "datetime") ? SqlDbType.DateTime : SqlDbType.SmallDateTime;
				break;
			case OleDbType.DBTimeStamp:
				cmdType = (typeName == "datetime") ? SqlDbType.DateTime : SqlDbType.SmallDateTime;
				break;
			case OleDbType.VarChar:
				cmdType = (typeName == "char") ? SqlDbType.Char : SqlDbType.VarChar;
				break;
			case OleDbType.LongVarChar:
				cmdType = SqlDbType.Text;
				break;
			case OleDbType.VarWChar:
				cmdType = (typeName == "nchar") ? SqlDbType.NChar : SqlDbType.NVarChar;
				break;
			case OleDbType.LongVarWChar:
				cmdType = SqlDbType.NText;
				break;
			case OleDbType.VarBinary:
				cmdType = (typeName == "binary") ? SqlDbType.Binary : SqlDbType.VarBinary;
				break;
			case OleDbType.LongVarBinary:
				cmdType = SqlDbType.Image;
				break;
		}
		return cmdType;
	}

	/// <summary>
	/// Converts the OleDb parameter direction
	/// </summary>
	/// <param name="oledbDirection">The integer parameter direction</param>
	/// <returns>A ParameterDirection</returns>
	private static ParameterDirection GetParameterDirection( short oledbDirection) 
	{
		ParameterDirection pd;
		switch (oledbDirection) 
		{
			case 1:
				pd = ParameterDirection.Input;
				break;
			case 2:
				pd = ParameterDirection.Output;
				break;
			case 4:
				pd = ParameterDirection.ReturnValue;
				break;
			default:
				pd = ParameterDirection.InputOutput;
				break;
		}
		return pd;
	}
}
#endregion