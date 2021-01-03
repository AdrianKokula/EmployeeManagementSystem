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
using EmployeeManagementSystem.Classes;
using EmployeeManagementSystem.Pages;

namespace EmployeeManagementSystem {

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private readonly Database database;
		private readonly string loggedUser;

		#region "constructors"

		public MainWindow(Database database, string loggedUser) {

			InitializeComponent();
			this.database = database;
			this.loggedUser = loggedUser;

			FrmMain.Content = new Employees(database);

		}

		#endregion

		#region "handlers"

		private void BtnEmployees_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Employees(database);
		}

		private void BtnDepartments_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Departments(database);
		}

		private void BtnUsers_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Users(database);
		}

		private void BtnSettings_Click(object sender, RoutedEventArgs e) {
			SettingsWindow settingsWindow = new SettingsWindow(database);
			settingsWindow.ShowDialog();
		}

		private void BtnAbout_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new About(database);
		}

		private void BtnExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		#endregion

		#region "methods"



		#endregion

	}
}
