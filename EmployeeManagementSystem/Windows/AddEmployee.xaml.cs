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
using System.Data;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for EditEmployeeWindow.xaml
	/// </summary>
	public partial class AddEmployee : Window {

		private readonly Database database;
		private readonly string loggedUser;

		#region "constructors"

		public AddEmployee(Database database, string loggedUser) {

			InitializeComponent();
			this.database = database;
			this.loggedUser = loggedUser;

			LoadData();

		}

		#endregion

		#region "handlers"
		
		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			CreateEmployee();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region "methods"

		#region "private"

		private void LoadData() {

			string query =
@"SELECT ID, CONCAT_WS(' ', DepartmentName + ' -', [State], City, PostalCode) AS Department
	FROM dbo.DepartmentView;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableDepartments = new DataTable();

			string result = database.FillDataTable(ref dataTableDepartments, sqlCommand);
			if (!result.Equals("OK")) return;

			CbDepartment.ItemsSource = dataTableDepartments.AsDataView();
			
		}

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbFirstName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbLastName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbResidence.Text)) return false;

			if (Tools.IntFromObject(CbDepartment.SelectedValue) <= 0) return false;

			return true;
		}

		private void CreateEmployee() {

			if (!ValidateFields()) return;

			

		}

		#endregion

		#endregion

	}

}
