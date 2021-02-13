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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditUser.xaml
	/// </summary>
	public partial class EditUser : UserControl {

		private readonly int userID;

		#region Constructors

		public EditUser(int userID) {

			InitializeComponent();

			this.userID = userID;

			Load();

		}

		#endregion

		#region Handlers

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			UpdateUser();
		}

		#endregion

		#region Methods

		#region Private methods

		private void Load() {

			string query =
@"DECLARE @UserID int = @pUserID;

SELECT [Name], Email, [Password]
	FROM dbo.Users
	WHERE ID = @UserID;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pUserID", SqlDbType.Int).Value = this.userID;

			DataTable dataTableUser = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableUser, sqlCommand);

			if (!result.Equals("OK")) return;
			if (dataTableUser.Rows.Count <= 0) return;

			DataRow row = dataTableUser.Rows[0];
			TbName.Text = Tools.StringFromObject(row["Name"]);
			TbEmail.Text = Tools.StringFromObject(row["Email"]);
			PbPassword.Password = Tools.StringFromObject(row["Password"]);

		}

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbEmail.Text)) return false;
			if (string.IsNullOrWhiteSpace(PbPassword.Password)) return false;

			return true;
		}

		private void UpdateUser() {

			if (!ValidateFields()) return;

			string query =
@"DECLARE @ID int = @pID;
DECLARE @UserName nvarchar(128) = @pUserName;
DECLARE @Email nvarchar(128) = @pEmail;
DECLARE @Password nvarchar(128) = @pPassword;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;
DECLARE @Result nvarchar(MAX);

EXEC dbo.UpdateUser @ID, @UserName, @Email, @Password, @LoggedUser, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = this.userID;
			sqlCommand.Parameters.Add("@pUserName", System.Data.SqlDbType.NVarChar).Value = TbName.Text;
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = TbEmail.Text;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = PbPassword.Password;
			sqlCommand.Parameters.Add("@pLoggedUser", System.Data.SqlDbType.NVarChar).Value = App.LoggedUser;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK")) return;

		}

		#endregion

		#endregion

	}
}
