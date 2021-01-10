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
		private readonly string loggedUser;

		#region "constructors"

		public EditEmployee(Database database, string loggedUser, int employeeID) {

			InitializeComponent();

			this.employeeID = employeeID;
			this.loggedUser = loggedUser;
			this.database = database;

			Load();

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			UpdateEmployee();
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

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbFirstName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbLastName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbResidence.Text)) return false;

			if (Tools.IntFromObject(CbDepartment.SelectedValue) <= 0) return false;

			if (!DpDateOfBirth.SelectedDate.HasValue) return false;

			return true;
		}

		private void UpdateEmployee() {

			if (!ValidateFields()) return;

			string query =
@"DECLARE @ID int = @pID;
DECLARE @FirstName nvarchar(128) = @pFirstName;
DECLARE @LastName nvarchar(128) = @pLastName;
DECLARE @DepartmentID int = @pDepartmentID;
DECLARE @DateOfBirth date = @pDateOfBirth;
DECLARE @PermanentResidence nvarchar(255) = @pPermanentResidence;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;

DECLARE @Result nvarchar(MAX);

EXEC dbo.UpdateEmployee @ID, @FirstName, @LastName, @DepartmentID, @DateOfBirth, @PermanentResidence, @LoggedUser, @Result OUTPUT

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = this.employeeID;
			sqlCommand.Parameters.Add("@pFirstName", SqlDbType.NVarChar).Value = TbFirstName.Text;
			sqlCommand.Parameters.Add("@pLastName", SqlDbType.NVarChar).Value = TbLastName.Text;
			sqlCommand.Parameters.Add("@pDepartmentID", SqlDbType.Int).Value = Tools.IntFromObject(CbDepartment.SelectedValue);
			sqlCommand.Parameters.Add("@pDateOfBirth", SqlDbType.Date).Value = DpDateOfBirth.SelectedDate.Value;
			sqlCommand.Parameters.Add("@pPermanentResidence", SqlDbType.NVarChar).Value = TbResidence.Text;
			sqlCommand.Parameters.Add("@pLoggedUser", SqlDbType.NVarChar).Value = this.loggedUser;

			string result = Tools.StringFromObject(database.Scalar(sqlCommand));
			if (!result.Equals("OK")) return;

		}

		#endregion

		#endregion

	}
}
