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

namespace EmployeeManagementSystem {
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window {

		#region "constructors"

		public Login() {
			InitializeComponent();
		}

		#endregion

		#region "handlers"
		private void BtnLogin_Click(object sender, RoutedEventArgs e) {
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}

		#endregion

	}
}
