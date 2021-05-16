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

			LoadData();

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

		private void LoadData() {

			string query =
@"SELECT ID, CONCAT_WS(' ', DepartmentName + ' -', [State], City, PostalCode) AS Department
	FROM dbo.DepartmentView;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableDepartments = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableDepartments, sqlCommand);
			if (!result.Equals("OK")) return;

			CbDepartment.ItemsSource = dataTableDepartments.AsDataView();
			
		}

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbFirstName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbLastName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbResidence.Text)) return false;

			if (Tools.IntFromObject(CbDepartment.SelectedValue) <= 0) return false;

			if (!DpDateOfBirth.SelectedDate.HasValue) return false;

			return true;
		}

		private void CreateEmployee() {

			if (!ValidateFields()) return;

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
			if (!result.Equals("OK")) return;

			Close();
		}

		#endregion

		#endregion

	}

}
