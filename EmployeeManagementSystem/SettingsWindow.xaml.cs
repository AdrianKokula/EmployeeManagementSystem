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
using EmployeeManagementSystem.Classes;
using System.Data.SqlClient;

namespace EmployeeManagementSystem {

	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window {

		#region Constructors

		public SettingsWindow() {

			InitializeComponent();

			InitControls();

		}

		#endregion

		#region Handlers

		private void BtnSaveSettings_Click(object sender, RoutedEventArgs e) {

			SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder() {
				DataSource = TbServerName.Text,
				InitialCatalog = TbDatabaseName.Text,
				UserID = TbDbUserName.Text,
				Password = TbDbUserPassword.Text,
				IntegratedSecurity = (bool)CbWindowsAuth.IsChecked
			};

			App.Database.ConnectionString = connectionStringBuilder.ConnectionString;

			if(!App.Database.TestConnection()) {
				MessageBox.Show("Connection couldn't be established. Please try again.", "Credentials", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Properties.Settings.Default.ConnectionString = App.Database.ConnectionString;
			Properties.Settings.Default.Save();

			Close();

		}

		private void CbWindowsAuth_CheckedChanged(object sender, RoutedEventArgs e) {

			if ((bool)CbWindowsAuth.IsChecked) {
				TbDbUserName.IsEnabled = false;
				TbDbUserPassword.IsEnabled = false;
			} else {
				TbDbUserName.IsEnabled = true;
				TbDbUserPassword.IsEnabled = true;
			}

		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region Methods

		#region Private methods

		/// <summary>
		/// Set values to window controls
		/// </summary>
		private void InitControls() {

			SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.ConnectionString);

			TbServerName.Text = connectionStringBuilder.DataSource;
			TbDatabaseName.Text = connectionStringBuilder.InitialCatalog;
			TbDbUserName.Text = connectionStringBuilder.UserID;
			TbDbUserPassword.Text = connectionStringBuilder.Password;
			CbWindowsAuth.IsChecked = connectionStringBuilder.IntegratedSecurity;

		}


		#endregion

		#endregion

	}
}
