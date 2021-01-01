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

		private readonly Database database;

		#region "constructors"

		public SettingsWindow(Database database) {

			InitializeComponent();
			this.database = database;

		}

		#endregion

		#region "handlers"

		private void BtnSaveSettings_Click(object sender, RoutedEventArgs e) {

			SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder() {
				DataSource = TbServerName.Text,
				InitialCatalog = TbDatabaseName.Text,
				UserID = TbDbUserName.Text,
				Password = TbDbUserPassword.Text,
				IntegratedSecurity = (bool)CbWindowsAuth.IsChecked
			};

			database.ConnectionString = connectionStringBuilder.ConnectionString;

			if(!database.TestConnection()) {
				MessageBox.Show("Connection couldn't be established. Please try again.", "Credentials", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			MessageBox.Show("Connection established.");

			Properties.Settings.Default.ConnectionString = database.ConnectionString;
			Properties.Settings.Default.Save();

			Close();

		}

		#endregion

	}
}
