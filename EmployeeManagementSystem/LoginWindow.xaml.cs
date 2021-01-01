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

namespace EmployeeManagementSystem {
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window {

		private readonly Database database;

		#region "constructors"

		public Login() {

			InitializeComponent();

			database = new Database(Properties.Settings.Default.ConnectionString);

			while (!database.TestConnection()) {
				SettingsWindow settingsWindow = new SettingsWindow(database);
				settingsWindow.ShowDialog();
			}

		}

		#endregion

		#region "handlers"

		private void BtnLogin_Click(object sender, RoutedEventArgs e) {
			MainWindow mainWindow = new MainWindow(database);
			mainWindow.Show();
			Close();
		}

		#endregion

	}
}
