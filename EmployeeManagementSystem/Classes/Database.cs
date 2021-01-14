// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EmployeeManagementSystem.Classes {

	public class Database {

		public string ConnectionString { get; set; }

		public Database(string connectionString) {
			this.ConnectionString = connectionString;
		}

		/// <summary>
		/// Test if given ConnectionString is valid for connection
		/// </summary>
		/// <returns>true - all good</returns>
		public bool TestConnection() {

			using(SqlConnection sqlConnection = new SqlConnection(this.ConnectionString)) {

				try {
					sqlConnection.Open();
				} catch(Exception) {
					return false;
				}

			}

			return true;
		}

		/// <summary>
		/// Execute sql command
		/// </summary>
		/// <param name="sqlComand"></param>
		/// <returns>true if all good</returns>
		public bool NonQuery(SqlCommand sqlCommand) {

			using (SqlConnection sqlConnection = new SqlConnection(this.ConnectionString)) {

				try {
					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;
					sqlCommand.ExecuteNonQuery();
				} catch (Exception) {
					return false;
				}

			}

			return true;
		}

		/// <summary>
		/// Receive table from sql query
		/// </summary>
		/// <param name="dataTable"></param>
		/// <param name="sqlCommand"></param>
		/// <returns>OK - all good, otherwise exception message</returns>
		public string FillDataTable(ref DataTable dataTable, SqlCommand sqlCommand) {

			using (SqlConnection sqlConnection = new SqlConnection(this.ConnectionString)) {

				try {

					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;

					using SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
					dataAdapter.Fill(dataTable);

				} catch (Exception ex) {
					return ex.Message;
				}

			}

			return "OK";
		}

		/// <summary>
		/// Receive single value from sql query
		/// </summary>
		/// <param name="sqlCommand"></param>
		/// <returns></returns>
		public object Scalar(SqlCommand sqlCommand) {

			object obj;

			using (SqlConnection sqlConnection = new SqlConnection(this.ConnectionString)) {

				try {

					sqlConnection.Open();
					sqlCommand.Connection = sqlConnection;
					obj = sqlCommand.ExecuteScalar();
					
				} catch (Exception) {
					return default;
				}

			}

			return obj;
		}

		/// <summary>
		/// Return binded SQL query for debugging
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static string GetBindedSqlCommand(SqlCommand command) {

			if (command.CommandType == CommandType.Text) {

				string parsedQuery = command.CommandText;

				// Parsing query
				foreach (SqlParameter parameter in command.Parameters) {

					// properly parsing query based on DB type
					switch (parameter.SqlDbType) {

						case SqlDbType.NVarChar:
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, "'" + Convert.ToString(parameter.Value) + "'");
							break;

						case SqlDbType.Bit:
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, Convert.ToBoolean(parameter.Value) == true ? "1" : "0");
							break;

						case SqlDbType sqlDbType
							when (sqlDbType == SqlDbType.Float || sqlDbType == SqlDbType.Money || sqlDbType == SqlDbType.Decimal):
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, Convert.ToString(parameter.Value).Replace(",", "."));
							break;

						case SqlDbType.Date:
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, "'" + Convert.ToDateTime(parameter.Value).ToString("yyyy-MM-dd") + "'");
							break;

						case SqlDbType.DateTime:
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, "'" + Convert.ToDateTime(parameter.Value).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'");
							break;

						default:
							parsedQuery = parsedQuery.Replace(parameter.ParameterName, Convert.ToString(parameter.Value));
							break;

					}

				}

				return parsedQuery;
			}

			return null;
		}


	}

}
