﻿// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System.Windows;
using System.Data.SqlClient;
using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem {

	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class LoginWindow : Window {

		#region Constructors

		public LoginWindow() {

			InitializeComponent();

			if(App.Database == null)
				App.Database = new Database(Properties.Settings.Default.ConnectionString);

			while (!App.Database.TestConnection()) {
				SettingsWindow settingsWindow = new SettingsWindow();
				_ = settingsWindow.ShowDialog();
			}

			TbEmail.Text = Properties.Settings.Default.LoginEmail;

			PbPassword.Password = "heslo";
			//Login();

		}

		#endregion

		#region Handlers

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		private void BtnLogin_Click(object sender, RoutedEventArgs e) {
			Login();
		}

		#endregion

		#region Methods

		#region Private methods

		private void Login() {

			string email = TbEmail.Text;
			string password = PbPassword.Password;
			string userName = CheckUser(email, password);

			if (string.IsNullOrWhiteSpace(userName)) {
				return;
			}

			Properties.Settings.Default.LoginEmail = (bool)CbRememberMail.IsChecked ? email : string.Empty;
			Properties.Settings.Default.Save();

			App.LoggedUser = userName;

			ShowMainWindow();

		}

		/// <summary>
		/// Checks if user with given credentials exists
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns>Username of specific user, empty if user dont exists</returns>
		private string CheckUser(string email, string password) {

			string query =
@"DECLARE @email nvarchar(128) = @pEmail;
DECLARE @password nvarchar(255) = @pPassword;

SELECT [dbo].[GetUsernameFromCredentials](@email, @password)";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = email;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = password;

			string userName = Tools.StringFromObject(App.Database.Scalar(sqlCommand));

			return userName;
		}

		private void ShowMainWindow() {

			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			Close();

		}

		#endregion

		#endregion

	}
}
