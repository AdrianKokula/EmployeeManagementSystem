// Copyright (c) 2020 Adrián Kokuľa - adriankokula.eu; License: The MIT License (MIT)

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data;

using System.Data.SqlClient;

using EmployeeManagementSystem.Classes;
using EmployeeManagementSystem.Windows;
using EmployeeManagementSystem.UserControls;

namespace EmployeeManagementSystem.Pages {

	/// <summary>
	/// Interaction logic for Employees.xaml
	/// </summary>
	public partial class Employees : Page {

		#region Constructors

		public Employees() {

			InitializeComponent();

			LoadData();

			EditEmp.InitUserControl(DeleteEmployee);

		}

		#endregion

		#region Handlers

		private void DgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			switch (DgEmployees.SelectedItems.Count) {
				case 0:
					BtnRemoveRecords.Visibility = Visibility.Collapsed;
					EditEmp.Visibility = Visibility.Collapsed;
					break;

				case 1:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					ShowSelectedRecord();
					break;

				default:
					BtnRemoveRecords.Visibility = Visibility.Visible;
					EditEmp.Visibility = Visibility.Collapsed;
					break;
			}

		}

		private void DgEmployees_KeyDown(object sender, KeyEventArgs e) {

			switch(e.Key) {

				case Key.Escape:
					DgEmployees.UnselectAll();
					EditEmp.Visibility = Visibility.Collapsed;
					break;

				case Key.Delete:
					DeleteSelectedRecords();
					break;

			}

		}

		private void BtnAddEmployee_Click(object sender, RoutedEventArgs e) {

			AddEmployee employeeWindow = new AddEmployee() {
				Owner = Window.GetWindow(this)
			};

			bool? dialogResult = employeeWindow.ShowDialog();
			if (!dialogResult.Value) {
				return;
			}
			
			LoadData();

		}

		private void BtnRemoveRecords_Click(object sender, RoutedEventArgs e) {
			DeleteSelectedRecords();
		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadData() {

			string query = @"SELECT * FROM dbo.EmployeeView;";
			SqlCommand sqlCommand = new SqlCommand(query);

			DataTable dataTableEmployees = new DataTable();
			string queryResult = App.Database.FillDataTable(ref dataTableEmployees, sqlCommand);

			if (!queryResult.Equals("OK", StringComparison.Ordinal)) {
				return;
			}

			DgEmployees.ItemsSource = dataTableEmployees.DefaultView;

		}

		private void DeleteSelectedRecords() {

			foreach (DataRowView selectedRow in DgEmployees.SelectedItems) {

				int employeeID = (int)selectedRow["ID"];
				DeleteEmployee(employeeID);

			}

		}

		private void DeleteEmployee(int employeeID) {

			if (employeeID <= 0) {
				return;
			}

			string query =
@"DECLARE @ID int = @pID;

DECLARE @Result nvarchar(MAX);
EXEC dbo.DeleteEmployee @ID, @Result OUTPUT;

SELECT @Result;";

			SqlCommand sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pID", SqlDbType.Int).Value = employeeID;

			string result = Tools.StringFromObject(App.Database.Scalar(sqlCommand));
			if (!result.Equals("OK", StringComparison.Ordinal)) {
				_ = MessageBox.Show(result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			EditEmp.Visibility = Visibility.Collapsed;
			LoadData();

		}

		private void ShowSelectedRecord() {

			if (DgEmployees.SelectedItems.Count <= 0) {
				return;
			}

			// first selected row 
			DataRowView selectedRow = (DataRowView)DgEmployees.SelectedItems[0];
			int employeeID = (int)selectedRow["ID"];

			EditEmp.EmployeeID = employeeID;
			EditEmp.Visibility = Visibility.Visible;

		}

		#endregion

		#endregion

	}
}
