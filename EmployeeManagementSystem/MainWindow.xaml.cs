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

		#region Constructors

		public MainWindow() {

			InitializeComponent();
			FrmMain.Content = new Employees();

		}

		#endregion

		#region Handlers

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void BtnMinimize_Click(object sender, RoutedEventArgs e) {
			WindowState = WindowState.Minimized;
		}

		private void BtnMaximize_Click(object sender, RoutedEventArgs e) {
			WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
		}

		private void BtnEmployees_Click(object sender, RoutedEventArgs e) {
			LoadEmpoyees();
		}

		private void BtnDepartments_Click(object sender, RoutedEventArgs e) {
			LoadDepartments();
		}

		private void BtnUsers_Click(object sender, RoutedEventArgs e) {
			LoadUsers();
		}

		private void BtnSettings_Click(object sender, RoutedEventArgs e) {
			SettingsWindow settingsWindow = new SettingsWindow();
			_ = settingsWindow.ShowDialog();
		}

		private void BtnAbout_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new About();
		}

		private void BtnExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		#endregion

		#region Methods

		private void LoadEmpoyees() {
			FrmMain.Content = new Employees();
			tbPageName.Text = "Employees";
			imgPageIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Employees.png"));
		}

		private void LoadDepartments() {
			FrmMain.Content = new Departments();
			tbPageName.Text = "Departments";
			imgPageIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Department.png"));
		}

		private void LoadUsers() {
			FrmMain.Content = new Users();
			tbPageName.Text = "Users";
			imgPageIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Login.png"));
		}

		#endregion

	}
}
