// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;
using EmployeeManagementSystem.Windows;

namespace EmployeeManagementSystem.Pages {

	/// <summary>
	/// Interaction logic for Departments.xaml
	/// </summary>
	public partial class Departments : Page {

		#region	Constructors

		public Departments() {

			InitializeComponent();

			LoadData();

			EditDep.InitUserControl(DeleteDepartment);

		}

		#endregion

		#region Handlers

		private void BtnAddDepartment_Click(object sender, RoutedEventArgs e) {

			AddDepartment departmentWindow = new AddDepartment() {
				Owner = Window.GetWindow(this)
			};

			bool? dialogResult = departmentWindow.ShowDialog();
			if (!dialogResult.Value) {
				return;
			}

			LoadData();

		}

		private void DgDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			if (e.AddedItems.Count == 0) {
				return;
			}

			// first selected row
			DataRowView dataRow = (DataRowView)e.AddedItems[0];
			int departmentID = (int)dataRow["ID"];

			EditDep.DepartmentID = departmentID;
			EditDep.Visibility = Visibility.Visible;

		}

		private void DgDepartments_KeyDown(object sender, KeyEventArgs e) {

			switch (e.Key) {

				case Key.Escape:
					DgDepartments.UnselectAll();
					EditDep.Visibility = Visibility.Collapsed;
					break;

			}

		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadData() {

			string query = @"SELECT * FROM dbo.DepartmentView;";
			SqlCommand sqlCommand = new SqlCommand(query);

			DataTable dataTableEmployees = new DataTable();
			string queryResult = App.Database.FillDataTable(ref dataTableEmployees, sqlCommand);

			if (!queryResult.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			DgDepartments.ItemsSource = dataTableEmployees.DefaultView;

		}

		private void DeleteDepartment(int departmentID) {
			if (departmentID <= 0) {
				return;
			}

			string query =
@"DECLARE @ID int = @pID;

DECLARE @Result nvarchar(MAX);
EXEC dbo.DeleteDepartment @ID, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = departmentID;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			EditDep.Visibility = Visibility.Collapsed;
			LoadData();

		}

		#endregion

		#endregion

	}
}
