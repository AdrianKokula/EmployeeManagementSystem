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

namespace EmployeeManagementSystem.Pages {
	/// <summary>
	/// Interaction logic for Users.xaml
	/// </summary>
	public partial class Users : Page {

		private readonly Database database;

		#region	"constructors"

		public Users(Database database) {

			InitializeComponent();
			this.database = database;

			LoadData();

		}

		#endregion

		#region "handlers"

		private void BtnAdd_Click(object sender, RoutedEventArgs e) {

			EditUserWindow userWindow = new EditUserWindow(this.database) {
				Owner = Window.GetWindow(this)
			};

			userWindow.ShowDialog();

		}

		#endregion

		#region "methods"

		#region "private"

		private void LoadData() {

			string query = @"SELECT ID, Name, Email FROM dbo.Users;";
			SqlCommand sqlCommand = new SqlCommand(query);

			DataTable dataTableEmployees = new DataTable();
			string queryResult = database.FillDataTable(ref dataTableEmployees, sqlCommand);

			if (!queryResult.Equals("OK")) return;

			LvUsers.ItemsSource = dataTableEmployees.DefaultView;

		}

		#endregion

		#endregion

	}
}
