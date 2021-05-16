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
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;

namespace EmployeeManagementSystem.Windows {

	/// <summary>
	/// Interaction logic for EditDepartmentWindow.xaml
	/// </summary>
	public partial class AddDepartment : Window {

		#region Constructors

		public AddDepartment() {

			InitializeComponent();

			LoadData();

		}

		#endregion

		#region Handlers

		private void BtnAddDepartment_Click(object sender, RoutedEventArgs e) {
			CreateDepartment();
		}

		private void BtnClose_Click(object sender, RoutedEventArgs e) {
			Close();
		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadData() {

			string query =
@"SELECT ID, NiceName [State]
	FROM dbo.States;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableStates = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableStates, sqlCommand);
			if (!result.Equals("OK")) return;

			CbState.ItemsSource = dataTableStates.AsDataView();

		}

		private bool ValidateFields() {

			if (string.IsNullOrWhiteSpace(TbName.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbCity.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbStreet.Text)) return false;
			if (string.IsNullOrWhiteSpace(TbPostalCode.Text)) return false;

			if (Tools.IntFromObject(CbState.SelectedValue) <= 0) return false;

			return true;
		}

		private void CreateDepartment() {

			if (!ValidateFields()) return;

			string query =
@"DECLARE @DepartmentName nvarchar(128) = @pDepartmentName;
DECLARE @StateID int = @pStateID;
DECLARE @City nvarchar(128) = @pCity;
DECLARE @Street nvarchar(128) = @pStreet;
DECLARE @PostalCode nvarchar(20) = @pPostalCode;
DECLARE @LoggedUser nvarchar(128) = @pLoggedUser;
DECLARE @Result nvarchar(MAX);

EXEC dbo.CreateDepartment @DepartmentName, @StateID, @City, @Street, @PostalCode, @LoggedUser, @Result OUTPUT;

SELECT @Result";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pDepartmentName", SqlDbType.NVarChar).Value = TbName.Text;
			sqlCommand.Parameters.Add("@pStateID", SqlDbType.Int).Value = Tools.IntFromObject(CbState.SelectedValue);
			sqlCommand.Parameters.Add("@pCity", SqlDbType.NVarChar).Value = TbCity.Text;
			sqlCommand.Parameters.Add("@pStreet", SqlDbType.NVarChar).Value = TbStreet.Text;
			sqlCommand.Parameters.Add("@pPostalCode", SqlDbType.NVarChar).Value = TbPostalCode.Text;
			sqlCommand.Parameters.Add("@pLoggedUser", SqlDbType.NVarChar).Value = App.LoggedUser;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK")) return;

			Close();
		}

		#endregion

		#endregion

	}
}
