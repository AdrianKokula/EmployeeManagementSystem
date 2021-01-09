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
	/// Interaction logic for Users.xaml
	/// </summary>
	public partial class Users : Page {

		private readonly Database database;
		private readonly string loggedUser;

		private EditUser editUser;

		#region	"constructors"

		public Users(Database database, string loggedUser) {

			InitializeComponent();

			this.database = database;
			this.loggedUser = loggedUser;

			LoadData();

		}

		#endregion

		#region "handlers"

		private void BtnAdd_Click(object sender, RoutedEventArgs e) {

			AddUser userWindow = new AddUser(this.database, this.loggedUser) {
				Owner = Window.GetWindow(this)
			};

			userWindow.ShowDialog();
			LoadData();

		}

		private void LvUsers_SelectionChanged(object sender, SelectionChangedEventArgs e) {

			// first selected row
			DataRowView dataRow = (DataRowView)e.AddedItems[0];
			int userID = (int)dataRow["ID"];

			editUser = new EditUser(database, userID);
			Grid.SetRow(editUser, 2);
			GridMain.Children.Add(editUser);

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
