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
	/// Interaction logic for Employees.xaml
	/// </summary>
	public partial class Employees : Page {

		private EditEmployee editEmployee;

		#region Constructors

		public Employees() {

			InitializeComponent();

			LoadData();

		}

		#endregion

		#region Handlers

		private void LvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			// first selected row
			DataRowView dataRow = (DataRowView)e.AddedItems[0];
			int employeeID = (int)dataRow["ID"];

			EditEmp.EmployeeID = employeeID;

		}

		private void BtnAdd_Click(object sender, RoutedEventArgs e) {

			AddEmployee employeeWindow = new AddEmployee() {
				Owner = Window.GetWindow(this)
			};

			employeeWindow.ShowDialog();
			LoadData();

		}

		#endregion

		#region Methods

		#region Private methods

		private void LoadData() {

			string query = @"SELECT * FROM dbo.EmployeeView;";
			SqlCommand sqlCommand = new SqlCommand(query);

			DataTable dataTableEmployees = new DataTable();
			string queryResult = App.Database.FillDataTable(ref dataTableEmployees, sqlCommand);

			if (!queryResult.Equals("OK")) return;

			LvEmployees.ItemsSource = dataTableEmployees.DefaultView;

		}

		#endregion

		#endregion

	}
}
