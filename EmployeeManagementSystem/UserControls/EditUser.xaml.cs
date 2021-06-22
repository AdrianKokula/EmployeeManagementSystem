// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditUser.xaml
	/// </summary>
	public partial class EditUser : UserControl {

		#region Delegates

		public delegate void DeleteUser(int userID);

		#endregion

		#region Private fields

		private int _UserID;

		private DeleteUser deleteUser;

		#endregion

		#region Properties

		public int UserID {
			get => _UserID;
			set {
				_UserID = value;
				LoadUser();
			}
		}

		#endregion

		#region Constructors

		public EditUser() {
			InitializeComponent();
		}

		#endregion

		#region Handlers

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			UpdateUser();
		}

		private void BtnDeleteRecord_Click(object sender, RoutedEventArgs e) {
			deleteUser(_UserID);
		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadUser() {

			string query =
@"DECLARE @UserID int = @pUserID;

SELECT [Name], Email, [Password]
	FROM dbo.Users
	WHERE ID = @UserID;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pUserID", SqlDbType.Int).Value = _UserID;

			DataTable dataTableUser = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableUser, sqlCommand);

			if (!result.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			if (dataTableUser.Rows.Count <= 0) {
				return;
			}

			DataRow row = dataTableUser.Rows[0];
			TbName.Text = Tools.StringFromObject(row["Name"]);
			TbEmail.Text = Tools.StringFromObject(row["Email"]);
			PbPassword.Password = Tools.StringFromObject(row["Password"]);

		}

		private bool ValidateFields() {

			if (_UserID <= 0) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(TbName.Text)) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(TbEmail.Text)) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(PbPassword.Password)) {
				return false;
			}

			return true;
		}

		private void UpdateUser() {

			if (!ValidateFields()) {
				return;
			}

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
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = _UserID;
			sqlCommand.Parameters.Add("@pUserName", SqlDbType.NVarChar).Value = TbName.Text;
			sqlCommand.Parameters.Add("@pEmail", SqlDbType.NVarChar).Value = TbEmail.Text;
			sqlCommand.Parameters.Add("@pPassword", SqlDbType.NVarChar).Value = PbPassword.Password;
			sqlCommand.Parameters.Add("@pLoggedUser", SqlDbType.NVarChar).Value = App.LoggedUser;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		#endregion

		#region Public methods

		public void InitUserControl(DeleteUser deleteUser) {

			Visibility = Visibility.Collapsed;
			this.deleteUser = deleteUser;

		}

		#endregion

		#endregion

	}
}
