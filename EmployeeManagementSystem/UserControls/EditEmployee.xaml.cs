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

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditEmployee.xaml
	/// </summary>
	public partial class EditEmployee : UserControl {

		private readonly Database database;
		private readonly int employeeID;

		#region "constructors"

		public EditEmployee(Database database, int employeeID) {

			InitializeComponent();

			this.employeeID = employeeID;
			this.database = database;

			Load();

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {

		}

		#endregion

		#region "methods"

		#region "private"

		private void Load() {

		}

		#endregion

		#endregion

	}
}
