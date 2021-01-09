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

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for EditDepartmentWindow.xaml
	/// </summary>
	public partial class AddDepartment : Window {

		private readonly Database database;
		private readonly string loggedUser;

		#region "constructors"

		public AddDepartment(Database database, string loggedUser) {

			InitializeComponent();
			this.database = database;
			this.loggedUser = loggedUser;

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {

		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region "methods"

		#region "private"



		#endregion

		#endregion

	}
}
