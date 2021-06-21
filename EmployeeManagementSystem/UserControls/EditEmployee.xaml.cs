// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using EmployeeManagementSystem.Classes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditEmployee.xaml
	/// </summary>
	public partial class EditEmployee : UserControl {

		private int _EmployeeID;

		#region Properties

		public int EmployeeID {
			get => _EmployeeID;
			set {
				_EmployeeID = value;
				LoadEmployee();
			}
		}

		#endregion

		#region Constructors

		public EditEmployee() {
			InitializeComponent();

			// Init controls - for instance combo boxes
			InitControls();

		}

		#endregion

		#region Handlers

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {
			UpdateEmployee();
		}

		private void BtnDeleteRecord_Click(object sender, RoutedEventArgs e) {
			DeleteEmployee();
		}

		#endregion

		#region Methods

		#region Private methods

		private void InitControls() {

			string query =
			@"SELECT ID, CONCAT_WS(' ', DepartmentName + ' -', [State], City, PostalCode) AS Department
	FROM dbo.DepartmentView;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableDepartments = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableDepartments, sqlCommand);
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			CbDepartment.ItemsSource = dataTableDepartments.AsDataView();

		}

		private void LoadEmployee() {

			string query =
@"DECLARE @EmployeeID int = @pEmployeeID;

SELECT FirstName, LastName, DepartmentID, DateOfBirth, PermanentResidence
	FROM dbo.Employees
	WHERE ID = @EmployeeID";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pEmployeeID", SqlDbType.Int).Value = _EmployeeID;

			DataTable dataTableEmployee = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableEmployee, sqlCommand);

			if (!result.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			if (dataTableEmployee.Rows.Count <= 0) {
				return;
			}

			DataRow row = dataTableEmployee.Rows[0];
			TbFirstName.Text = Tools.StringFromObject(row["FirstName"]);
			TbLastName.Text = Tools.StringFromObject(row["LastName"]);
			CbDepartment.SelectedValue = Tools.IntFromObject(row["DepartmentID"]);
			DpDateOfBirth.SelectedDate = Tools.DateTimeFromObject(row["DateOfBirth"]);
			TbResidence.Text = Tools.StringFromObject(row["PermanentResidence"]);

		}

		private bool ValidateFields() {

			if (_EmployeeID <= 0) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(TbFirstName.Text)) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(TbLastName.Text)) {
				return false;
			}

			if (string.IsNullOrWhiteSpace(TbResidence.Text)) {
				return false;
			}

			if (Tools.IntFromObject(CbDepartment.SelectedValue) <= 0) {
				return false;
			}

			if (!DpDateOfBirth.SelectedDate.HasValue) {
				return false;
			}

			return true;
		}

		private void UpdateEmployee() {

			if (!ValidateFields()) {
				return;
			}

			string query =
@"DECLARE @ID int = @pID;
DECLARE @FirstName nvarchar(128) = @pFirstName;
DECLARE @LastName nvarchar(128) = @pLastName;
DECLARE @DepartmentID int = @pDepartmentID;
DECLARE @DateOfBirth date = @pDateOfBirth;
DECLARE @PermanentResidence nvarchar(255) = @pPermanentResidence;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;

DECLARE @Result nvarchar(MAX);
EXEC dbo.UpdateEmployee @ID, @FirstName, @LastName, @DepartmentID, @DateOfBirth, @PermanentResidence, @LoggedUser, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = _EmployeeID;
			sqlCommand.Parameters.Add("@pFirstName", SqlDbType.NVarChar).Value = TbFirstName.Text;
			sqlCommand.Parameters.Add("@pLastName", SqlDbType.NVarChar).Value = TbLastName.Text;
			sqlCommand.Parameters.Add("@pDepartmentID", SqlDbType.Int).Value = Tools.IntFromObject(CbDepartment.SelectedValue);
			sqlCommand.Parameters.Add("@pDateOfBirth", SqlDbType.Date).Value = DpDateOfBirth.SelectedDate.Value;
			sqlCommand.Parameters.Add("@pPermanentResidence", SqlDbType.NVarChar).Value = TbResidence.Text;
			sqlCommand.Parameters.Add("@pLoggedUser", SqlDbType.NVarChar).Value = App.LoggedUser;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

		}

		private void DeleteEmployee() {

			if (_EmployeeID <= 0) {
				return;
			}

			string query =
@"DECLARE @ID int = @pID;

DECLARE @Result nvarchar(MAX);
EXEC dbo.DeleteEmployee @ID, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = _EmployeeID;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

		}

		#endregion

		#endregion

	}
}
