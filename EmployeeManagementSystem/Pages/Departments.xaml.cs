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
using EmployeeManagementSystem.Windows;
using EmployeeManagementSystem.UserControls;

namespace EmployeeManagementSystem.Pages {

	/// <summary>
	/// Interaction logic for Departments.xaml
	/// </summary>
	public partial class Departments : Page {

		#region	Constructors

		public Departments() {

			InitializeComponent();

			LoadData();

			EditDep.Visibility = Visibility.Collapsed;

		}

		#endregion

		#region Handlers

		private void BtnAddDepartment_Click(object sender, RoutedEventArgs e) {

			AddDepartment departmentWindow = new AddDepartment() {
				Owner = Window.GetWindow(this)
			};

			departmentWindow.ShowDialog();

			LoadData();

		}

		private void DgDepartments_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			if (e.AddedItems.Count == 0) return;

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

			if (!queryResult.Equals("OK")) return;

			DgDepartments.ItemsSource = dataTableEmployees.DefaultView;

		}

		#endregion

		#endregion

	}
}
