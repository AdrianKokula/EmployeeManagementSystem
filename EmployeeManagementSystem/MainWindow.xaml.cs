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

		private void Window_MouseDown(object sender, MouseButtonEventArgs e) {

			if (e.ChangedButton == MouseButton.Left) {
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

		private void BtnEmployees_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Employees();
		}

		private void BtnDepartments_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Departments();
		}

		private void BtnUsers_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new Users();
		}

		private void BtnSettings_Click(object sender, RoutedEventArgs e) {
			SettingsWindow settingsWindow = new SettingsWindow();
			settingsWindow.ShowDialog();
		}

		private void BtnAbout_Click(object sender, RoutedEventArgs e) {
			FrmMain.Content = new About();
		}

		private void BtnExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		#endregion

		#region Methods


		#endregion

	}
}
