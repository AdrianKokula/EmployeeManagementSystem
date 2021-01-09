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

namespace EmployeeManagementSystem.UserControls {

	/// <summary>
	/// Interaction logic for EditDepartment.xaml
	/// </summary>
	public partial class EditDepartment : UserControl {

		private readonly Database database;
		private readonly int departmentID;

		#region "constructors"

		public EditDepartment(Database database, int departmentID) {

			InitializeComponent();

			this.database = database;
			this.departmentID = departmentID;

			Load();

		}

		#endregion

		#region "handlers"

		private void BtnSubmit_Click(object sender, RoutedEventArgs e) {

		}

		#endregion

		#region "methods"

		#region "private"

		private void Load() {

			// load combo box
			string query =
@"SELECT ID, NiceName [State]
	FROM dbo.States;";

			SqlCommand sqlCommand = new SqlCommand(query);
			DataTable dataTableStates = new DataTable();

			string result = database.FillDataTable(ref dataTableStates, sqlCommand);
			if (!result.Equals("OK")) return;

			CbState.ItemsSource = dataTableStates.AsDataView();

			//load department
			query =
@"DECLARE @DepartmentID int = @pDepartmentID;

SELECT [Name], StateID, City, Street, PostalCode
	FROM dbo.Departments
	WHERE ID = @DepartmentID;";

			sqlCommand = new SqlCommand(query);
			sqlCommand.Parameters.Add("@pDepartmentID", SqlDbType.Int).Value = this.departmentID;

			DataTable dataTableDepartment = new DataTable();
			result = database.FillDataTable(ref dataTableDepartment, sqlCommand);

			if (!result.Equals("OK")) return;
			if (dataTableDepartment.Rows.Count <= 0) return;

			DataRow row = dataTableDepartment.Rows[0];
			TbName.Text = Tools.StringFromObject(row["Name"]);
			CbState.SelectedValue = Tools.IntFromObject(row["StateID"]);
			TbCity.Text = Tools.StringFromObject(row["City"]);
			TbStreet.Text = Tools.StringFromObject(row["Street"]);
			TbPostalCode.Text = Tools.StringFromObject(row["PostalCode"]);

		}

		#endregion

		#endregion

	}
}
