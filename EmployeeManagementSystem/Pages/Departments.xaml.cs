// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Collections.Generic;
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

		private void BtnRemoveRecords_Click(object sender, RoutedEventArgs e) {
			DeleteSelectedRecords();
		}

		private void DgDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			switch (DgDepartments.SelectedItems.Count) {
				case 0:
					BtnRemoveRecords.Visibility = Visibility.Collapsed;
					EditDep.Visibility = Visibility.Collapsed;
					break;

				case 1:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					ShowSelectedRecord();
					break;

				default:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					EditDep.Visibility = Visibility.Collapsed;
					break;
			}

		}

		private void DgDepartments_PreviewKeyDown(object sender, KeyEventArgs e) {

			switch (e.Key) {

				case Key.Escape:
					DgDepartments.UnselectAll();
					EditDep.Visibility = Visibility.Collapsed;
					break;

				case Key.Delete:
					DeleteSelectedRecords();
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

		private void DeleteSelectedRecords() {

			if (DgDepartments.SelectedItems.Count <= 0) {
				return;
			}

			List<DataRowView> list = new List<DataRowView>(DgDepartments.SelectedItems.Count);

			foreach (DataRowView selectedRow in DgDepartments.SelectedItems) {
				list.Add(selectedRow);
			}

			list.ForEach(row => {
				int departmentID = (int)row["ID"];
				DeleteDepartment(departmentID);
			});

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

		private void ShowSelectedRecord() {

			if (DgDepartments.SelectedItems.Count <= 0) {
				return;
			}

			// first selected row 
			DataRowView selectedRow = (DataRowView)DgDepartments.SelectedItems[0];
			int departmentID = (int)selectedRow["ID"];

			EditDep.DepartmentID = departmentID;
			EditDep.Visibility = Visibility.Visible;

		}

		#endregion

		#endregion

	}
}
