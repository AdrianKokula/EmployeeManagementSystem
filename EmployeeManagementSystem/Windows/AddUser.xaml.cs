// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for AddUser.xaml
	/// </summary>
	public partial class AddUser : Window {

		private readonly Database database;
		private readonly string loggedUser;

		#region "constructors"

		public AddUser(Database database, string loggedUser) {

			InitializeComponent();
			this.database = database;
			this.loggedUser = loggedUser;

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			CreateUser();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region "methods"

		#region "private"

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbEmail.Text)) return false;
			if (string.IsNullOrWhiteSpace(PbPassword.Password)) return false;

			return true;
		}

		private void CreateUser() {

			if (!ValidateFields()) return;

			string query =
@"DECLARE @UserName nvarchar(128) = @pUserName;
DECLARE @Email nvarchar(128) = @pEmail;
DECLARE @Password nvarchar(128) = @pPassword;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;
DECLARE @Result nvarchar(MAX);

EXEC dbo.CreateUser @UserName, @Email, @Password, @LoggedUser, @Result OUTPUT

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pUserName", System.Data.SqlDbType.NVarChar).Value = TbName.Text;
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = TbEmail.Text;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = PbPassword.Password;
			sqlCommand.Parameters.Add("@pLoggedUser", System.Data.SqlDbType.NVarChar).Value = this.loggedUser;

			string result = Tools.StringFromObject(database.Scalar(sqlCommand));
			if (!result.Equals("OK")) return;

			Close();

		}

		#endregion

		#endregion


	}
}
