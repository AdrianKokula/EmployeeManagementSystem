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

namespace EmployeeManagementSystem {
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class LoginWindow : Window {

		private readonly Database database;

		#region "constructors"

		public LoginWindow() {

			InitializeComponent();

			database = new Database(Properties.Settings.Default.ConnectionString);

			while (!database.TestConnection()) {
				SettingsWindow settingsWindow = new SettingsWindow(database);
				settingsWindow.ShowDialog();
			}

			// only for dev
			TbEmail.Text = "kokula.adrian45@gmail.com";
			PbPassword.Password = "heslo";
			Login();

		}

		#endregion

		#region "handlers"

		private void BtnLogin_Click(object sender, RoutedEventArgs e) {
			Login();
		}

		#endregion

		#region "methods"

		#region "private"

		private void Login() {

			string email = TbEmail.Text;
			string password = PbPassword.Password;
			string userName = CheckUser(email, password);

			if (string.IsNullOrWhiteSpace(userName)) return;

			ShowMainWindow(userName);

		}

		/// <summary>
		/// Checks if user with given credentials exists
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns>Username of specific user, empty if user dont exists</returns>
		private string CheckUser(string email, string password) {

			string query =
@"DECLARE @email nvarchar(255) = @pEmail;
DECLARE @password nvarchar(255) = @pPassword;

SELECT [dbo].[GetUsernameFromCredentials](@email, @password)";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = email;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = password;

			string userName = DbTools.StringFromSqlScalar(database.Scalar(sqlCommand));

			return userName;
		}

		private void ShowMainWindow(string userName) {

			MainWindow mainWindow = new MainWindow(database);
			mainWindow.Show();
			Close();

		}

		#endregion

		#endregion

	}
}
