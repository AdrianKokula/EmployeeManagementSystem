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

		#region Constructors

		public AddEmployee() {

			InitializeComponent();

			SpErrorEnterFirstName.Visibility = Visibility.Collapsed;
			SpErrorEnterLastName.Visibility = Visibility.Collapsed;
			SpErrorSelectDepartment.Visibility = Visibility.Collapsed;
			SpErrorSelectDateOfBirth.Visibility = Visibility.Collapsed;
			SpErrorDateOfBirthNotValid.Visibility = Visibility.Collapsed;
			SpErrorEnterResidence.Visibility = Visibility.Collapsed;

			InitControls();

		}

		#endregion

		#region Handlers
		
		private void BtnAddEmployee_Click(object sender, RoutedEventArgs e) {
			CreateEmployee();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
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

		private bool ValidateFields() {

			bool result = true;

			if (string.IsNullOrWhiteSpace(TbFirstName.Text)) {
				SpErrorEnterFirstName.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterFirstName.Visibility = Visibility.Collapsed;
			}

			if (string.IsNullOrWhiteSpace(TbLastName.Text)) {
				SpErrorEnterLastName.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterLastName.Visibility = Visibility.Collapsed;
			}

			if (Tools.IntFromObject(CbDepartment.SelectedValue) <= 0) {
				SpErrorSelectDepartment.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorSelectDepartment.Visibility = Visibility.Collapsed;
			}

			if (!DpDateOfBirth.SelectedDate.HasValue) {
				SpErrorSelectDateOfBirth.Visibility = Visibility.Visible;
				SpErrorDateOfBirthNotValid.Visibility = Visibility.Collapsed;
				result = false;
			} else {

				SpErrorSelectDateOfBirth.Visibility = Visibility.Collapsed;

				if (DpDateOfBirth.SelectedDate.Value.Date.CompareTo(DateTime.Now.Date) > 0) {
					SpErrorDateOfBirthNotValid.Visibility = Visibility.Visible;
					result = false;
				} else {
					SpErrorDateOfBirthNotValid.Visibility = Visibility.Collapsed;
				}

			}

			if (string.IsNullOrWhiteSpace(TbResidence.Text)) {
				SpErrorEnterResidence.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterResidence.Visibility = Visibility.Collapsed;
			}

			return result;
		}

		private void CreateEmployee() {

			if (!ValidateFields()) {
				return;
			}

			string query =
@"DECLARE @FirstName nvarchar(128) = @pFirstName;
DECLARE @LastName nvarchar(128) = @pLastName;
DECLARE @DepartmentID int = @pDepartmentID;
DECLARE @DateOfBirth date = @pDateOfBirth;
DECLARE @PermanentResidence nvarchar(255) = @pPermanentResidence;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;

DECLARE @Result nvarchar(MAX);

EXEC dbo.CreateEmployee @FirstName, @LastName, @DepartmentID, @DateOfBirth, @PermanentResidence, @LoggedUser, @Result OUTPUT

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
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

			DialogResult = true;
			Close();
		}

		#endregion

		#endregion

	}

}
