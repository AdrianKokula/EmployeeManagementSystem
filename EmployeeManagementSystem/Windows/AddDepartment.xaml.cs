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

			SpErrorEnterName.Visibility = Visibility.Collapsed;
			SpErrorSelectState.Visibility = Visibility.Collapsed;
			SpErrorEnterCity.Visibility = Visibility.Collapsed;
			SpErrorEnterStreet.Visibility = Visibility.Collapsed;
			SpErrorEnterPostalCode.Visibility = Visibility.Collapsed;

			InitControls();

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

		private void InitControls() {

			string query =
@"SELECT ID, NiceName [State]
	FROM dbo.States;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableStates = new DataTable();

			string result = App.Database.FillDataTable(ref dataTableStates, sqlCommand);
			if (!result.Equals("OK", StringComparison.Ordinal)) return;

			CbState.ItemsSource = dataTableStates.AsDataView();

		}

		private bool ValidateFields() {

			bool result = true;

			if (string.IsNullOrWhiteSpace(TbName.Text)) {
				SpErrorEnterName.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterName.Visibility = Visibility.Collapsed;
			}

			if (Tools.IntFromObject(CbState.SelectedValue) <= 0) {
				SpErrorSelectState.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorSelectState.Visibility = Visibility.Collapsed;
			}

			if (string.IsNullOrWhiteSpace(TbCity.Text)) {
				SpErrorEnterCity.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterCity.Visibility = Visibility.Collapsed;
			}

			if (string.IsNullOrWhiteSpace(TbStreet.Text)) {
				SpErrorEnterStreet.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterStreet.Visibility = Visibility.Collapsed;
			}

			if (string.IsNullOrWhiteSpace(TbPostalCode.Text)) {
				SpErrorEnterPostalCode.Visibility = Visibility.Visible;
				result = false;
			} else {
				SpErrorEnterPostalCode.Visibility = Visibility.Collapsed;
			}

			return result;
		}

		private void CreateDepartment() {

			if (!ValidateFields()) {
				return;
			}

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
