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

			TbEmail.Text = Properties.Settings.Default.LoginEmail;

		}

		#endregion

		#region "handlers"

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {

			if(e.ChangedButton == MouseButton.Left) {
				this.Cursor = Cursors.SizeAll;
				this.DragMove();
			}

		}

		private void Window_MouseUp(object sender, MouseButtonEventArgs e) {
			this.Cursor = Cursors.Arrow;
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

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

			if (CbRememberMail.IsChecked == true) {
				Properties.Settings.Default.LoginEmail = email;
			} else {
				Properties.Settings.Default.LoginEmail = String.Empty;
			}

			Properties.Settings.Default.Save();

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
@"DECLARE @email nvarchar(128) = @pEmail;
DECLARE @password nvarchar(255) = @pPassword;

SELECT [dbo].[GetUsernameFromCredentials](@email, @password)";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pEmail", System.Data.SqlDbType.NVarChar).Value = email;
			sqlCommand.Parameters.Add("@pPassword", System.Data.SqlDbType.NVarChar).Value = password;

			string userName = Tools.StringFromObject(database.Scalar(sqlCommand));

			return userName;
		}

		private void ShowMainWindow(string userName) {

			MainWindow mainWindow = new MainWindow(database, userName);
			mainWindow.Show();
			Close();

		}

		#endregion

		#endregion

	}
}
