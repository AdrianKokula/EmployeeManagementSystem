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
using System.Data;
using System.Data.SqlClient;

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

			// load combo box
			string query =
			@"SELECT ID, CONCAT_WS(' ', DepartmentName + ' -', [State], City, PostalCode) AS Department
	FROM dbo.DepartmentView;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableDepartments = new DataTable();

			string result = database.FillDataTable(ref dataTableDepartments, sqlCommand);
			if (!result.Equals("OK")) return;

			CbDepartment.ItemsSource = dataTableDepartments.AsDataView();

			// load employee
			query =
@"DECLARE @EmployeeID int = @pEmployeeID;

SELECT FirstName, LastName, DepartmentID, DateOfBirth, PermanentResidence
	FROM dbo.Employees
	WHERE ID = @EmployeeID";

			sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pEmployeeID", SqlDbType.Int).Value = this.employeeID;

			DataTable dataTableEmployee = new DataTable();

			result = database.FillDataTable(ref dataTableEmployee, sqlCommand);

			if (!result.Equals("OK")) return;
			if (dataTableEmployee.Rows.Count <= 0) return;

			DataRow row = dataTableEmployee.Rows[0];
			TbFirstName.Text = Tools.StringFromObject(row["FirstName"]);
			TbLastName.Text = Tools.StringFromObject(row["LastName"]);
			CbDepartment.SelectedValue = Tools.IntFromObject(row["DepartmentID"]);
			DpDateOfBirth.SelectedDate = Tools.DateTimeFromObject(row["DateOfBirth"]);
			TbResidence.Text = Tools.StringFromObject(row["PermanentResidence"]);

		}

		#endregion

		#endregion

	}
}
