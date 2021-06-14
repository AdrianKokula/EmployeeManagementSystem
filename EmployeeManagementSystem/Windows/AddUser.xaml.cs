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
using System.ComponentModel.DataAnnotations;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for AddUser.xaml
	/// </summary>
	public partial class AddUser : Window {

		#region Constructors

		public AddUser() {

			InitializeComponent();

			SpErrorEnterName.Visibility = Visibility.Collapsed;
			SpErrorEnterEmail.Visibility = Visibility.Collapsed;
			SpErrorEnterValidEmail.Visibility = Visibility.Collapsed;
			SpErrorEnterPassword.Visibility = Visibility.Collapsed;
			SpErrorConfrimPassword.Visibility = Visibility.Collapsed;
			SpErrorPasswordDidntMatch.Visibility = Visibility.Collapsed;

		}

		#endregion

		#region Handlers

		private void BtnAddUser_Click(object sender, RoutedEventArgs e) {
			CreateUser();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region Methods

		#region Private methods

		private bool ValidateFields() {

			bool result = true;

			// check name
			if (string.IsNullOrWhiteSpace(TbName.Text)) {
				SpErrorEnterName.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterName.Visibility = Visibility.Collapsed;
			}

			// check email
			if (string.IsNullOrWhiteSpace(TbEmail.Text)) {
				SpErrorEnterEmail.Visibility = Visibility.Visible;
				result = false;
			} else { // check if entered email is valid
				SpErrorEnterEmail.Visibility = Visibility.Collapsed;

				EmailAddressAttribute emailAddressAttribute = new EmailAddressAttribute();
				bool validEmail = emailAddressAttribute.IsValid(TbEmail.Text);

				if (!validEmail) {
					SpErrorEnterValidEmail.Visibility = Visibility.Visible;
					result = false;
				} else {
					SpErrorEnterValidEmail.Visibility = Visibility.Collapsed;
				}

			}

			// check password
			if (string.IsNullOrWhiteSpace(PbPassword.Password)) {
				SpErrorEnterPassword.Visibility = Visibility.Visible;
				SpErrorPasswordDidntMatch.Visibility = Visibility.Collapsed;
				SpErrorConfrimPassword.Visibility = Visibility.Collapsed;
				result = false;
			} else { // check if password is confirmed
				SpErrorEnterPassword.Visibility = Visibility.Collapsed;

				if (string.IsNullOrWhiteSpace(PbPasswordAgain.Password)) {
					SpErrorConfrimPassword.Visibility = Visibility.Visible;
					result = false;
				} else { // both passwords are entered, check password match
					SpErrorConfrimPassword.Visibility = Visibility.Collapsed;

					if (!PbPassword.Password.Equals(PbPasswordAgain.Password, StringComparison.Ordinal)) {
						SpErrorPasswordDidntMatch.Visibility = Visibility.Visible;
						result = false;
					} else {
						SpErrorPasswordDidntMatch.Visibility = Visibility.Collapsed;
					}

				}

			}

			return result;
		}

		private void CreateUser() {

			if (!ValidateFields()) {
				return;
			}

			string query =
@"DECLARE @UserName nvarchar(128) = @pUserName;
DECLARE @Email nvarchar(128) = @pEmail;
DECLARE @Password nvarchar(128) = @pPassword;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;
DECLARE @Result nvarchar(MAX);

EXEC dbo.CreateUser @UserName, @Email, @Password, @LoggedUser, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pUserName", System.Data.SqlDbType.NVarChar).Value = TbName.Text;
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = TbEmail.Text;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = PbPassword.Password;
			sqlCommand.Parameters.Add("@pLoggedUser", System.Data.SqlDbType.NVarChar).Value = App.LoggedUser;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			Close();
		}

		#endregion

		#endregion

	}
}
